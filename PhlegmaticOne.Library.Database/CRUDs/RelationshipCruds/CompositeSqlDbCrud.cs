using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipCruds;

public class CompositeSqlDbCrud : SqlDbCrud
{
    public CompositeSqlDbCrud(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                              DataContextConfigurationBase<AdoDataService> configuration) :
                              base(connection, expressionProvider, configuration)
    { }

    public override async Task<int?> AddAsync<TEntity>(TEntity entity)
    {
        var id = await new ToAnotherSqlDbCrud(Connection, ExpressionProvider, Configuration).AddAsync(entity);
        return await new ToManySqLDbCrud(Connection, ExpressionProvider, Configuration).AddInTempTable(entity, id);
    }

    public override async Task<TEntity> GetFull<TEntity>(int id)
    {
        var lazyToAnotherEntity =
            await new ToAnotherSqlDbCrud(Connection, ExpressionProvider, Configuration).GetFull<TEntity>(id);
        var lazyToManyEntity =
            await new ToManySqLDbCrud(Connection, ExpressionProvider, Configuration).GetFull<TEntity>(id);
        var relatedObjects = lazyToManyEntity.PropertiesWithAppearance<IEnumerable<DomainModelBase>>().First();
        relatedObjects.SetValue(lazyToAnotherEntity, relatedObjects.GetValue(lazyToManyEntity));
        return lazyToAnotherEntity;
    }
}