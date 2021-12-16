using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddUpdate;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddUpdate.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs;
/// <summary>
/// Represents instance for getting SqlDbAddUpdate instance depends of relationship of entity
/// </summary>
public class SqlDbAddUpdateFactory
{
    private readonly IRelationshipIdentifier _relationshipIdentifier;
    private readonly ISqlCommandExpressionProvider _expressionProvider;
    private readonly DataContextConfigurationBase<AdoDataService> _configuration;
    private readonly IRelationShipResolver _relationShipResolver;
    /// <summary>
    /// Initializes new SqlDbAddUpdateFactory instance
    /// </summary>
    public SqlDbAddUpdateFactory(IRelationshipIdentifier relationshipIdentifier,
                             ISqlCommandExpressionProvider expressionProvider,
                             DataContextConfigurationBase<AdoDataService> configuration,
                             IRelationShipResolver relationShipResolver)
    {
        _relationshipIdentifier = relationshipIdentifier;
        _expressionProvider = expressionProvider;
        _configuration = configuration;
        _relationShipResolver = relationShipResolver;
    }
    /// <summary>
    /// Gets SqlDbAddUpdate instance depends of relationship of entity
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="connection">Sql connection</param>
    public SqlDbAddUpdate<TEntity> SqlCrudFor<TEntity>(SqlConnection connection)
        where TEntity : DomainModelBase => _relationshipIdentifier.IdentifyRelationship<TEntity>() switch
        {
            ObjectRelationship.Single => new SingleSqlDbAdding<TEntity>(connection, _expressionProvider, _configuration, _relationShipResolver),
            ObjectRelationship.ToAnother => new ToAnotherSqlDbAdding<TEntity>(connection, _expressionProvider, _configuration, _relationShipResolver),
            ObjectRelationship.ToMany => new ToManySqLDbAdding<TEntity>(connection, _expressionProvider, _configuration, _relationShipResolver),
            ObjectRelationship.Composite => new CompositeSqlDbAdding<TEntity>(connection, _expressionProvider, _configuration, _relationShipResolver),
            _ => throw new ArgumentException()
        };
}