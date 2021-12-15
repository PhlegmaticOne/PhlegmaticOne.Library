using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data;
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
            dynamic relatedObjects = relatedEntitiesCollection.GetValue(entity);
            if(relatedObjects.Count == 0) continue;
            var entityType = entity.GetType();
            var tableName = Configuration.TableNames[entityType];
            var relatedObjectsType = relatedObjects[0].GetType();
            foreach (var relatedObject in relatedObjects)
            {
                var relatedObjectId = await GetIdOfExisting(relatedObject);
                var properties = new Dictionary<string, object>()
                {
                    { Configuration.ForeignPropertyNameFor(entityType), id },
                    { Configuration.ForeignPropertyNameFor(relatedObjectsType), relatedObjectId }
                };
                var table = SqlHelper.ToFilledDataTable(Connection, ExpressionProvider.EmptySelectFor(tableName),
                                                        out SqlDataAdapter adapter, out DataSet dataSet);
                table.Rows.Add(table.NewRow().ParametrizeWith(properties));
                SqlHelper.SaveChanges(adapter, dataSet);
            }
        }
        return id;
    }
}