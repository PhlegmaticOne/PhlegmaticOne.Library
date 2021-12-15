using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs;

public class SqlDbAddingFactory
{
    private readonly IRelationshipIdentifier _relationshipIdentifier;
    private readonly ISqlCommandExpressionProvider _expressionProvider;
    private readonly DataContextConfigurationBase<AdoDataService> _configuration;
    private readonly IRelationShipResolver _relationShipResolver;

    public SqlDbAddingFactory(IRelationshipIdentifier relationshipIdentifier,
                             ISqlCommandExpressionProvider expressionProvider,
                             DataContextConfigurationBase<AdoDataService> configuration,
                             IRelationShipResolver relationShipResolver)
    {
        _relationshipIdentifier = relationshipIdentifier;
        _expressionProvider = expressionProvider;
        _configuration = configuration;
        _relationShipResolver = relationShipResolver;
    }

    public SqlDbAdding<TEntity> SqlCrudFor<TEntity>(SqlConnection connection)
        where TEntity : DomainModelBase => _relationshipIdentifier.IdentifyRelationship<TEntity>() switch
        {
            ObjectRelationship.Single => new SingleSqlDbAdding<TEntity>(connection, _expressionProvider, _configuration, _relationShipResolver),
            ObjectRelationship.ToAnother => new ToAnotherSqlDbAdding<TEntity>(connection, _expressionProvider, _configuration, _relationShipResolver),
            ObjectRelationship.ToMany => new ToManySqLDbAdding<TEntity>(connection, _expressionProvider, _configuration, _relationShipResolver),
            ObjectRelationship.Composite => new CompositeSqlDbAdding<TEntity>(connection, _expressionProvider, _configuration, _relationShipResolver),
            _ => throw new ArgumentException()
        };
}