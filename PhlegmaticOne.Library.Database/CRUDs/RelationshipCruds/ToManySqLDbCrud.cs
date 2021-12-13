using System.Collections.Generic;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipCruds;

public class ToManySqLDbCrud : SqlDbCrud
{
    public ToManySqLDbCrud(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                            DataContextConfigurationBase<AdoDataService> configuration) :
                            base(connection, expressionProvider, configuration)
    { }

    public override async Task<int?> AddAsync<TEntity>(TEntity entity)
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
                                                               await AddInTempTable(entity, configuredEntity.Result.Result.Value)).Result,
                    _ => await base.AddAsync(entity)
                };
            }
        }
    }

    public override async Task<TEntity> GetFull<TEntity>(int id)
    {
        switch (Configuration.ManyToManyAddingEntity)
        {
            case TEntity:
            {
                var lazyEntity = await GetLazy<TEntity>(id);
                var relatedObjects = lazyEntity.PropertiesWithAppearance<IEnumerable<DomainModelBase>>().First();
                var entityTypeName = typeof(TEntity).Name;
                var relatedEntitiesType = relatedObjects.PropertyType.GenericTypeArguments.First();
                var relatedObjectsTypeName = relatedEntitiesType.Name;
                var tableName = entityTypeName + "s" + relatedObjectsTypeName + "s";
                var expression =
                    ExpressionProvider.SelectFromManyToManyTable(id, tableName, entityTypeName + "Id",
                                                        relatedObjectsTypeName + "Id");
                var sqlAdapter = new SqlDataAdapter(expression, Connection);
                var dataSet = new DataSet();
                var relatedEntities = new List<DomainModelBase>();
                sqlAdapter.Fill(dataSet);
                var table = dataSet.Tables.First();
                foreach (DataRow row in table.Rows)
                {
                    var relatedId = (int)row.ItemArray[0];
                    var relatedEntity = await ToGeneric(relatedEntitiesType, relatedId);
                    relatedEntities.Add(relatedEntity);
                }
                relatedObjects.SetValue(lazyEntity, relatedEntities.Cast<Author>());
                return lazyEntity;
            }
            default: throw new ArgumentException();
        }
    }

    public async Task<int?> AddInTempTable<TEntity>(TEntity entity, int? id) where TEntity : DomainModelBase
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