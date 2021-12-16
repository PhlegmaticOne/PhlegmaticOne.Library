using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings.Base;

public abstract class SqlDbAdding<TEntity> where TEntity : DomainModelBase
{
    protected readonly SqlConnection Connection;
    protected readonly ISqlCommandExpressionProvider ExpressionProvider;
    protected readonly DataContextConfigurationBase<AdoDataService> Configuration;
    protected readonly IRelationShipResolver RelationShipResolver;

    protected SqlDbAdding(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                          DataContextConfigurationBase<AdoDataService> configuration, IRelationShipResolver relationShipResolver)
    {
        Connection = connection;
        ExpressionProvider = expressionProvider;
        Configuration = configuration;
        RelationShipResolver = relationShipResolver;
    }
    public virtual async Task<int?> AddAsync(TEntity entity) =>
        await Task.Factory
            .StartNew(() => AddConfiguredEntity(entity))
            .ContinueWith(_ =>
                SqlHelper.ExecuteReadingOneNumberCommand(ExpressionProvider.GetLastIdFor<TEntity>(), Connection)).Result;

    public virtual async Task UpdateAsync(TEntity oldEntity, TEntity newEntity)
    {
        var oldId = oldEntity.Id;
        var expr = ExpressionProvider.UpdateExpression(oldId, newEntity);
        await using var command = new SqlCommand(expr, Connection);
        await command.ExecuteNonQueryAsync();
    }

    protected void AddConfiguredEntity(TEntity entity)
    {
        var entityProperties =
            RelationShipResolver.ToNoRelationshipProperties(typeof(TEntity).GetProperties())
                                .Where(p => p.Name != Configuration.IdPropertyName);
        var propertiesNamesAndValues = entityProperties.ToDictionary(key => key.Name, value => value.GetValue(entity));
        var table = SqlHelper.ToFilledDataTable(Connection, ExpressionProvider.EmptySelectFor<TEntity>(),
                                                out var adapter, out var dataSet);
        table.AddRowWith(propertiesNamesAndValues);
        SqlHelper.SaveChanges(adapter, dataSet);
    }
    protected async Task<int?> GetIdOfExisting(DomainModelBase entity) =>
        await SqlHelper.ExecuteReadingOneNumberCommand(ExpressionProvider.SelectIdExpression(entity), Connection);
}