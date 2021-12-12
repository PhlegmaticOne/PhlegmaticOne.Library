using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipCruds;

public class SingleSqlDbCrud : SqlDbCrud
{
    public SingleSqlDbCrud(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
        DataContextConfigurationBase<AdoDataService> configuration) :
        base(connection, expressionProvider, configuration) { }
}