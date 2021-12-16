using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings;

public class CompositeSqlDbAdding<TEntity> : SqlDbAdding<TEntity> where TEntity : DomainModelBase
{
    public CompositeSqlDbAdding(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                                DataContextConfigurationBase<AdoDataService> configuration, IRelationShipResolver relationShipResolver) :
                                base(connection, expressionProvider, configuration, relationShipResolver)
    { }
    public override async Task<int?> AddAsync(TEntity entity)
    {
        var id = await new ToAnotherSqlDbAdding<TEntity>(Connection, ExpressionProvider, Configuration, RelationShipResolver)
            .AddAsync(entity);
        return await new ToManySqLDbAdding<TEntity>(Connection, ExpressionProvider, Configuration, RelationShipResolver)
            .AddInTempTable(entity, id);
    }
    public override async Task UpdateAsync(TEntity oldEntity, TEntity newEntity)
    {
        await new ToAnotherSqlDbAdding<TEntity>(Connection, ExpressionProvider, Configuration, RelationShipResolver)
            .UpdateAsync(oldEntity, newEntity);
        await new ToManySqLDbAdding<TEntity>(Connection, ExpressionProvider, Configuration, RelationShipResolver)
            .UpdateAsync(oldEntity, newEntity);
    }
}