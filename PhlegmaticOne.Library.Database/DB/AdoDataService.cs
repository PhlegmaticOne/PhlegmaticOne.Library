using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.CRUDs;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using PhlegmaticOne.Library.Domain.Services;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.DB;

public class AdoDataService : IDataService, IAsyncDisposable
{
    private readonly SqlDbAddUpdateFactory _sqlDbCrudsFactory;
    private SqlConnection _connection;
    private IRelationshipIdentifier _relationshipIdentifier;
    private ISqlCommandExpressionProvider _sqlCommandExpressionProvider;
    public DataContextConfigurationBase<AdoDataService> DataContextConfiguration { get; }
    private AdoDataService(SqlDbAddUpdateFactory sqlDbCrudsFactory, IRelationshipIdentifier relationshipIdentifier,
                           ISqlCommandExpressionProvider sqlCommandExpressionProvider,
                           DataContextConfigurationBase<AdoDataService> dataContextConfiguration)
    {
        _sqlDbCrudsFactory = sqlDbCrudsFactory;
        _relationshipIdentifier = relationshipIdentifier;
        _sqlCommandExpressionProvider = sqlCommandExpressionProvider;
        DataContextConfiguration = dataContextConfiguration;
    }
    /// <summary>
    /// Creates new AdoDataService instance async 
    /// </summary>
    /// <param name="connectionStringGetter">Connection string getter</param>
    /// <param name="sqlDbCrudsFactory">Sql database operations factory</param>
    /// <param name="dataContextConfiguration">Database configuration</param>
    /// <param name="relationshipIdentifier">Relationship identifier</param>
    /// <param name="sqlCommandExpressionProvider">Command expression provider</param>
    /// <returns></returns>
    internal static async Task<AdoDataService> CreateInstanceAsync(IConnectionStringGetter connectionStringGetter,
                                                                   SqlDbAddUpdateFactory sqlDbCrudsFactory,
                                                                   DataContextConfigurationBase<AdoDataService> dataContextConfiguration,
                                                                   IRelationshipIdentifier relationshipIdentifier,
                                                                   ISqlCommandExpressionProvider sqlCommandExpressionProvider)
    {
        var instance = new AdoDataService(sqlDbCrudsFactory, relationshipIdentifier, sqlCommandExpressionProvider, dataContextConfiguration);
        instance._connection = new SqlConnection(connectionStringGetter.GetConnectionString());
        await instance._connection.OpenAsync();
        return instance;
    }
    public async Task<int?> AddAsync<TEntity>(TEntity entity) where TEntity : DomainModelBase =>
        await _sqlDbCrudsFactory.SqlCrudFor<TEntity>(_connection).AddAsync(entity);
    public async Task<TEntity?> GetLazyAsync<TEntity>(int id) where TEntity : DomainModelBase =>
        await new SelectingAlgorithms(_connection, _sqlCommandExpressionProvider, DataContextConfiguration)
                 .SelectSingle(id, typeof(TEntity)) as TEntity;
    public async Task<TEntity?> GetFullAsync<TEntity>(int id) where TEntity : DomainModelBase
    {
        var selectingAlgorithms = new SelectingAlgorithms(_connection, _sqlCommandExpressionProvider, DataContextConfiguration);
        return _relationshipIdentifier.IdentifyRelationship<TEntity>() switch
        {
            ObjectRelationship.Single => await selectingAlgorithms.SelectSingle(id, typeof(TEntity)) as TEntity,
            ObjectRelationship.ToAnother => await selectingAlgorithms.SelectToAnother(id, typeof(TEntity)) as TEntity,
            ObjectRelationship.ToMany or ObjectRelationship.Composite =>
                await selectingAlgorithms.SelectComposite(id, typeof(TEntity)) as TEntity,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    public async Task<int> DeleteAsync<TEntity>(int id) where TEntity : DomainModelBase
    {
        await using var command = new SqlCommand(_sqlCommandExpressionProvider.DeleteExpression<TEntity>(id), _connection);
        return await command.ExecuteNonQueryAsync();
    }
    public async Task UpdateAsync<TEntity>(int id, TEntity newEntity) where TEntity : DomainModelBase
    {
        var oldEntity = await GetFullAsync<TEntity>(id);
        await _sqlDbCrudsFactory.SqlCrudFor<TEntity>(_connection).UpdateAsync(oldEntity, newEntity);
    }
    /// <summary>
    /// Deletes all data from tables in database
    /// </summary>
    public async Task<int> EnsureDeletedAsync()
    {
        var list = new List<string>();
        await using var readCommand = new SqlCommand(SqlCommandsPatterns.AllDeletingStatements, _connection);
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