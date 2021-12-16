using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings;

public class ToManySqLDbAdding<T> : SqlDbAdding<T> where T : DomainModelBase
{
    public ToManySqLDbAdding(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                             DataContextConfigurationBase<AdoDataService> configuration, IRelationShipResolver relationShipResolver) :
                             base(connection, expressionProvider, configuration, relationShipResolver)
    { }
    public override async Task<int?> AddAsync(T entity)
    {
        return Configuration.ManyToManyAddingType switch
        {
            ManyToManyAddingType.ForeignPropertiesMustExist => await Task.Factory
                .StartNew(async () => await base.AddAsync(entity))
                .ContinueWith(async (configuredEntity) =>
                    await AddInTempTable(entity, configuredEntity.Result.Result.Value)).Result,
            _ => throw new ArgumentException()
        };
    }
    public async Task<int?> AddInTempTable(T entity, int? id)
    {
        foreach (var relatedEntitiesCollection in RelationShipResolver.ToManyToManyProperties(entity))
        {
            var relatedObjects = relatedEntitiesCollection.GetValue(entity) as IEnumerable<DomainModelBase>;
            if (relatedObjects.Any() == false) continue;
            await AddEntitiesInTempTable(relatedObjects, entity,
                relatedEntitiesCollection.PropertyType.GenericTypeArguments[0], id.Value);
        }
        return id;
    }

    public override async Task UpdateAsync(T oldEntity, T newEntity)
    {
        switch (Configuration)
        {
            case { ManyToManyUpdatingType: ManyToManyUpdatingType.AddDeleteRelatedEntitiesWhenChanged }:
                {
                    foreach (var relatedEntitiesCollection in RelationShipResolver.ToManyToManyProperties(newEntity))
                    {
                        var newEntityRelatedEntities = relatedEntitiesCollection.GetValue(newEntity) as IEnumerable<DomainModelBase>;
                        var oldEntityRelatedEntities = relatedEntitiesCollection.GetValue(oldEntity) as IEnumerable<DomainModelBase>;
                        var relatedEntitiesType = relatedEntitiesCollection.PropertyType.GenericTypeArguments[0];
                        if (newEntityRelatedEntities.Count() == oldEntityRelatedEntities.Count()) continue;
                        if (newEntityRelatedEntities.Count() > oldEntityRelatedEntities.Count())
                        {
                            await AddEntitiesInTempTable(newEntityRelatedEntities.Except(oldEntityRelatedEntities),
                                newEntity, relatedEntitiesType, newEntity.Id);
                        }
                        else
                        {
                            var entityType = newEntity.GetType();
                            var tableName = Configuration.TableNames[entityType];
                            foreach (var newRelatedObject in oldEntityRelatedEntities.Except(newEntityRelatedEntities))
                            {
                                var expression = ExpressionProvider.DeleteFromManyToManyTableExpression(tableName,
                                    Configuration.ForeignPropertyNameFor(relatedEntitiesType),
                                    Configuration.ForeignPropertyNameFor(entityType),
                                    newRelatedObject.Id, oldEntity.Id);
                                await SqlHelper.ExecuteVoidCommand(expression, Connection);
                            }
                        }
                    }
                    break;
                }
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private async Task AddEntitiesInTempTable(IEnumerable<DomainModelBase> relatedProperties,
        DomainModelBase primaryEntity, Type foreignEntityType, int id = 0)
    {
        var entityType = primaryEntity.GetType();
        var tableName = Configuration.TableNames[entityType];
        foreach (var newRelatedObject in relatedProperties)
        {
            var relatedObjectId = await GetIdOfExisting(newRelatedObject);
            var properties = new Dictionary<string, object?>
            {
                { Configuration.ForeignPropertyNameFor(entityType), id },
                { Configuration.ForeignPropertyNameFor(foreignEntityType), relatedObjectId }
            };
            var table = SqlHelper.ToFilledDataTable(Connection, ExpressionProvider.EmptySelectFor(tableName),
                out var adapter, out var dataSet);
            table.AddRowWith(properties);
            SqlHelper.SaveChanges(adapter, dataSet);
        }
    }
}