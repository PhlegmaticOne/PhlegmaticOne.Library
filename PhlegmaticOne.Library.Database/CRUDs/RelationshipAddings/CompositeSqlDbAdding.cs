using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings;

public class CompositeSqlDbAdding<TEntity> : SqlDbAdding<TEntity> where TEntity: DomainModelBase
{
    public CompositeSqlDbAdding(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                              DataContextConfigurationBase<AdoDataService> configuration) :
                              base(connection, expressionProvider, configuration)
    { }

    public override async Task<int?> AddAsync(TEntity entity)
    {
        var id = await new ToAnotherSqlDbAdding<TEntity>(Connection, ExpressionProvider, Configuration).AddAsync(entity);
        return await new ToManySqLDbAdding<TEntity>(Connection, ExpressionProvider, Configuration).AddInTempTable(entity, id);
    }
}