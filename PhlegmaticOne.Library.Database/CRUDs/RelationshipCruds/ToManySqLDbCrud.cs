using System.Data;
using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipCruds;

public class ToManySqLDbCrud : SqlDbCrud
{
    public ToManySqLDbCrud(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                            DataContextConfigurationBase<AdoDataService> configuration) :
                            base(connection, expressionProvider, configuration) { }

    public override async Task<int> AddAsync<TEntity>(TEntity entity)
    {
        switch (Configuration.ManyToManyAddingType)
        {
            case ManyToManyAddingType.ForeignPropertiesMustExist:
            default:
            {
                return Configuration.ManyToManyAddingEntity switch
                {
                    TEntity => await Task.Factory.StartNew(async () => await base.AddAsync(entity))
                                                 .ContinueWith(async (configuredEntity) =>
                                                               await AddInTempTable(entity, configuredEntity.Result.Result)).Result,
                    _ => await base.AddAsync(entity)
                };
            }
        }
    }

    public async Task<int> AddInTempTable<TEntity>(TEntity entity, int id) where TEntity: DomainModelBase
    {
        var relatedObjects = typeof(TEntity).GetProperties()
            .First(p => p.PropertyType.IsAssignableTo(typeof(IEnumerable<DomainModelBase>)))
            .GetValue(entity) as IEnumerable<DomainModelBase>;
        var entityTypeName = typeof(TEntity).Name;
        var relatedObjectsTypeName = relatedObjects.First().GetType().Name;
        var tableName = entityTypeName + "s" + relatedObjectsTypeName + "s";
        foreach (var relatedObject in relatedObjects)
        {
            var relatedObjectId = await GetIdOfExisting(relatedObject);
            var properties = new Dictionary<string, object>()
            {
                { entityTypeName + "Id", id },
                { relatedObjectsTypeName + "Id", relatedObjectId }
            };
            var adapter = new SqlDataAdapter(SqlCommandsPatterns.EmptySelectFor(tableName), Connection);
            var dataSet = new DataSet(); adapter.Fill(dataSet);
            var table = dataSet.Tables.First();
            table.Rows.Add(table.NewRow().ParametrizeWith(properties));
            new SqlCommandBuilder(adapter);
            adapter.Update(dataSet);
        }
        return id;
    }
}