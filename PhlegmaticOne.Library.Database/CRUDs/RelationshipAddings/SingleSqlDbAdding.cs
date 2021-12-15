using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings;

public class SingleSqlDbAdding<T> : SqlDbAdding<T> where T: DomainModelBase
{
    public SingleSqlDbAdding(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
        DataContextConfigurationBase<AdoDataService> configuration) :
        base(connection, expressionProvider, configuration)
    { }
}