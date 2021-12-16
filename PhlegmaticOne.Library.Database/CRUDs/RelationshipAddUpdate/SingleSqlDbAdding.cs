using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddUpdate.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipAddUpdate;
/// <summary>
/// Represent instance for adding and updating single entities
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class SingleSqlDbAdding<TEntity> : SqlDbAddUpdate<TEntity> where TEntity : DomainModelBase
{
    /// <summary>
    /// Initializes new SingleSqlDbAdding instance
    /// </summary>
    public SingleSqlDbAdding(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                             DataContextConfigurationBase<AdoDataService> configuration, IRelationShipResolver relationShipResolver) :
                             base(connection, expressionProvider, configuration, relationShipResolver)
    { }
}