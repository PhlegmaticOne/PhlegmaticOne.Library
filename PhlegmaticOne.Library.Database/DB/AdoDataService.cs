using System.Data;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.CRUDs;
using PhlegmaticOne.Library.Domain.Models;
using PhlegmaticOne.Library.Domain.Services;
using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Configuration;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.Relationships;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;

namespace PhlegmaticOne.Library.Database.DB;

public class AdoDataService : IDataService, IAsyncDisposable
{
    private readonly SqlDbCrudsFactory _sqlDbCrudsFactory;
    private SqlConnection _connection;
    private IRelationshipIdentifier _relationshipIdentifier = new RelationshipIdentifier();
    private ISqlCommandExpressionProvider _sqlCommandExpressionProvider = new SqlCommandExpressionProvider();
    private DataContextConfigurationBase<AdoDataService> _dataContextConfiguration = new AdoDataContextConfiguration();
    private AdoDataService(SqlDbCrudsFactory sqlDbCrudsFactory) => _sqlDbCrudsFactory = sqlDbCrudsFactory;

    internal static async Task<AdoDataService> CreateInstanceAsync(IConnectionStringGetter connectionStringGetter,
                                                                   SqlDbCrudsFactory sqlDbCrudsFactory)
    {
        var instance = new AdoDataService(sqlDbCrudsFactory);
        instance._connection = new SqlConnection(connectionStringGetter.GetConnectionString());
        await instance._connection.OpenAsync();
        return instance;
    }
    public async Task<int?> AddAsync<TEntity>(TEntity entity) where TEntity : DomainModelBase =>
        await _sqlDbCrudsFactory.SqlCrudFor<TEntity>(_connection).AddAsync(entity);
    public async Task<TEntity?> GetLazyAsync<TEntity>(int id) where TEntity : DomainModelBase =>
        await new SelectingAlgorithms(_connection, _relationshipIdentifier, _sqlCommandExpressionProvider, _dataContextConfiguration)
                 .SelectSingle(id, typeof(TEntity)) as TEntity;

    public async Task<int> DeleteAsync<TEntity>(int id) where TEntity : DomainModelBase
    {
        await using var command = new SqlCommand(_sqlCommandExpressionProvider.DeleteExpression<TEntity>(id), _connection);
        return await command.ExecuteNonQueryAsync();
    }
    public async Task<int?> UpdateAsync<TEntity>(int id, TEntity newEntity) where TEntity : DomainModelBase => 
        await DeleteAsync<TEntity>(id) != 0 ? await AddAsync(newEntity) : -1;
    public async Task<TEntity?> GetFullAsync<TEntity>(int id) where TEntity : DomainModelBase
    {
        var algorithms = new SelectingAlgorithms(_connection, _relationshipIdentifier, _sqlCommandExpressionProvider, _dataContextConfiguration);
        return _relationshipIdentifier.IdentifyRelationship<TEntity>() switch
        {
            ObjectRelationship.Single => await algorithms.SelectSingle(id, typeof(TEntity)) as TEntity,
            ObjectRelationship.ToAnother => await algorithms.SelectToAnother(id, typeof(TEntity)) as TEntity,
            ObjectRelationship.ToMany => await algorithms.SelectToMany(id, typeof(TEntity)) as TEntity,
            ObjectRelationship.Composite => await algorithms.SelectComposite(id, typeof(TEntity)) as TEntity,
            _ => throw new ArgumentException()
        };
    }
    public async Task<int> GetIdOfExisting<TEntity>(TEntity entity) where TEntity : DomainModelBase
    {
        var expression = _sqlCommandExpressionProvider.SelectIdExpression(entity);
        var reader = await new SqlCommand(expression, _connection).ExecuteReaderAsync();
        await reader.ReadAsync();
        return reader.GetInt32(0);
    }

    public async Task<int> EnsureDeletedAsync()
    {
        var list = new List<string>();
        await using var readCommand = new SqlCommand(SqlCommandsPatterns.AllDeletingStatements(), _connection);
        await using (var rdr = await readCommand.ExecuteReaderAsync())
        {
            while (await rdr.ReadAsync()) list.Add(rdr.GetString(0));
        }
        var totalDeleted = 0;
        foreach (var commandText in list)
        {
            await using var command = new SqlCommand(commandText, _connection);
            totalDeleted += await command.ExecuteNonQueryAsync();
        }
        return totalDeleted;
    }
    internal async Task<IEnumerable<object>> ExecuteCommand(string commandText)
    {
        var result = new List<object>();
        await using var command = new SqlCommand(commandText, _connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                result.Add(reader.GetValue(i));
            }
        }
        return result;
    }
    public ValueTask DisposeAsync() => _connection.DisposeAsync();
    ~AdoDataService() => DisposeAsync();
}

internal class SelectingAlgorithms
{
    private readonly SqlConnection _connection;
    private readonly ISqlCommandExpressionProvider _expressionProvider;
    private readonly IRelationshipIdentifier _identifier;
    private readonly DataContextConfigurationBase<AdoDataService> _dataContextConfiguration;
    internal SelectingAlgorithms(SqlConnection connection, IRelationshipIdentifier identifier,
                                 ISqlCommandExpressionProvider expressionProvider,
                                 DataContextConfigurationBase<AdoDataService> dataContextConfiguration)
    {
        _connection = connection;
        _identifier = identifier;
        _expressionProvider = expressionProvider;
        _dataContextConfiguration = dataContextConfiguration;
    }
    internal async Task<DomainModelBase?> SelectSingle(int id, Type entityType)
    {
        return await Task.Run(() =>
        {
            var entity = Activator.CreateInstance(entityType) as DomainModelBase;
            var properties = entityType.GetProperties();
            var expression = _expressionProvider.SelectLazyByIdExpression(id, entityType);
            var table = SqlHelper.ToFilledDataTable(_connection, expression);
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    var value = row[column.ColumnName];
                    properties.First(p => p.Name == column.ColumnName)
                              .SetValue(entity, row.IsNull(column) ? null : value);
                }
            }
            return entity;
        });
    }
    internal async Task<DomainModelBase> SelectToAnother(int id, Type entityType)
    {
        var lazyEntity = await SelectSingle(id, entityType);
        var entityProperties = lazyEntity.GetType().GetProperties();
        foreach (var property in lazyEntity.PropertiesWithAppearance<DomainModelBase>())
        {
            var relatedEntitiesType = property.PropertyType;
            var relatedId = entityProperties.First(p => p.Name.Contains(property.Name + "Id")).GetValue(lazyEntity);
            if(relatedId is null) continue;
            var relatedEntity = _identifier.IdentifyRelationship(relatedEntitiesType) switch
            {
                ObjectRelationship.ToAnother => await SelectToAnother((int)relatedId, relatedEntitiesType),
                ObjectRelationship.ToMany => await SelectToMany((int)relatedId, relatedEntitiesType),
                ObjectRelationship.Composite => await SelectComposite((int)relatedId, relatedEntitiesType),
                _ => await SelectSingle((int)relatedId, relatedEntitiesType),
            };
            property.SetValue(lazyEntity, relatedEntity);
        }
        return lazyEntity;
    }
    internal async Task<DomainModelBase> SelectToMany(int id, Type entityType, DomainModelBase configuringEntity = null)
    {
        var initialConfiguringEntity = Activator.CreateInstance(entityType) as DomainModelBase;
        var relatedObjectsCollectionInToManyRelationship = initialConfiguringEntity.PropertiesWithAppearance<IEnumerable<DomainModelBase>>().First();
        DomainModelBase? relatedEntity = default;
        if (entityType != _dataContextConfiguration.ManyToManyAddingEntity.GetType())
        {
            var relatedObjects = initialConfiguringEntity.PropertiesWithAppearance<DomainModelBase>();
            relatedEntity = relatedObjects.Count() == 0 ? await SelectSingle(id, entityType) : await SelectToAnother(id, entityType);
            relatedObjectsCollectionInToManyRelationship.PropertyType.GetMethod("Add")
                .Invoke(relatedEntity.PropertiesWithAppearance<IEnumerable<DomainModelBase>>().First().GetValue(relatedEntity),
                    new object[] { configuringEntity });
            return relatedEntity;
        }
        var relatedEntities = new List<DomainModelBase>();
        var relatedEntitiesType = relatedObjectsCollectionInToManyRelationship.PropertyType.GenericTypeArguments.First();
        var expression = _expressionProvider.SelectFromManyToManyTable(id, entityType.Name + "s" + relatedEntitiesType.Name + "s",
                                                              entityType.Name + "Id", relatedEntitiesType.Name + "Id");
        foreach (DataRow row in SqlHelper.ToFilledDataTable(_connection, expression).Rows)
        {
            var relatedId = (int)row.ItemArray[0];
            relatedEntity = _identifier.IdentifyRelationship(relatedEntitiesType) switch
            {
                ObjectRelationship.ToAnother => await SelectToAnother(relatedId, relatedEntitiesType),
                ObjectRelationship.ToMany => await SelectToMany(relatedId, relatedEntitiesType, initialConfiguringEntity),
                ObjectRelationship.Composite => await SelectComposite(relatedId, relatedEntitiesType),
                _ => await SelectSingle(relatedId, relatedEntitiesType),
            };
            relatedEntities.Add(relatedEntity);
        }
        relatedObjectsCollectionInToManyRelationship.SetValue(initialConfiguringEntity, relatedEntities.CastTo(relatedEntitiesType));
        return initialConfiguringEntity;
    }
    internal async Task<DomainModelBase> SelectComposite(int id, Type entityType)
    {
        var toAnotherEntity = await SelectToAnother(id, entityType);
        var toManyEntity = await SelectToMany(id, entityType);
        var relatedObjects = toManyEntity.PropertiesWithAppearance<IEnumerable<DomainModelBase>>().First();
        relatedObjects.SetValue(toAnotherEntity, relatedObjects.GetValue(toManyEntity));
        return toAnotherEntity;
    }
}

internal class AddingAlgorithms
{

}