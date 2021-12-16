using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs;

internal class SelectingAlgorithms
{
    private readonly SqlConnection _connection;
    private readonly ISqlCommandExpressionProvider _expressionProvider;
    private readonly DataContextConfigurationBase<AdoDataService> _configuration;
    private readonly IRelationShipResolver _relationShipResolver;
    private Type RetrievingType;
    private bool _isConfiguring;

    internal SelectingAlgorithms(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                                 DataContextConfigurationBase<AdoDataService> dataContextConfiguration)
    {
        _connection = connection;
        _expressionProvider = expressionProvider;
        _configuration = dataContextConfiguration;
        _relationShipResolver = new RelationshipResolver(dataContextConfiguration);
    }
    internal async Task<DomainModelBase> SelectComposite(int id, Type entityType, DomainModelBase configuringEntity = null)
    {
        var toAnotherEntity = await SelectToAnother(id, entityType);
        var toManyEntity = await SelectToMany(id, entityType, configuringEntity);
        foreach (var relatedObjectsInCollection in _relationShipResolver.ToManyToManyProperties(toManyEntity))
        {
            relatedObjectsInCollection.SetValue(toAnotherEntity, relatedObjectsInCollection.GetValue(toManyEntity));
        }
        return toAnotherEntity;
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
        foreach (var property in _relationShipResolver.ToToAnotherProperties(lazyEntity))
        {
            var relatedEntitiesType = property.PropertyType;
            var relatedId = entityProperties
                .First(p => p.Name == _configuration.ForeignPropertyNameFor(property.PropertyType)).GetValue(lazyEntity);
            if (relatedId is null) continue;
            var relatedEntity = await SelectComposite((int)relatedId, relatedEntitiesType);
            property.SetValue(lazyEntity, relatedEntity);
        }
        return lazyEntity;
    }

    internal async Task<DomainModelBase> SelectToMany(int id, Type entityType, DomainModelBase configuringEntity = null)
    {
        var initialConfiguringEntity = Activator.CreateInstance(entityType) as DomainModelBase;
        foreach (var foreignObjectsCollection in _relationShipResolver.ToManyToManyProperties(initialConfiguringEntity))
        {
            if (_isConfiguring == false) RetrievingType = entityType;
            _isConfiguring = true;
            DomainModelBase? relatedEntity = default;
            if (RetrievingType == entityType && configuringEntity is not null)
            {
                relatedEntity = _relationShipResolver.ToToAnotherProperties(initialConfiguringEntity).Any() ?
                    await SelectToAnother(id, entityType) : await SelectSingle(id, entityType);
                var foreignEntities = foreignObjectsCollection.GetValue(relatedEntity);
                foreignEntities.GetType().GetMethod("Add")
                    .Invoke(foreignEntities, new object?[] { configuringEntity });
                return relatedEntity;
            }
            var relatedEntities = new List<DomainModelBase>();
            var relatedEntitiesType = foreignObjectsCollection.PropertyType.GenericTypeArguments.First();
            var expression = _expressionProvider.SelectFromManyToManyTable(id, _configuration.TableNames[entityType],
                                    _configuration.ForeignPropertyNameFor(entityType),
                                                    _configuration.ForeignPropertyNameFor(relatedEntitiesType));
            foreach (DataRow row in SqlHelper.ToFilledDataTable(_connection, expression).Rows)
            {
                var relatedId = (int)row.ItemArray[0];
                relatedEntity = await SelectComposite(relatedId, relatedEntitiesType, initialConfiguringEntity);
                relatedEntities.Add(relatedEntity);
            }
            foreignObjectsCollection.SetValue(initialConfiguringEntity,
                _relationShipResolver.CastTo(relatedEntities, relatedEntitiesType));
            _isConfiguring = false;
        }
        return initialConfiguringEntity;
    }
}