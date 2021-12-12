using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipCruds;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.CRUDs;

public class SqlDbCrudsFactory
{
    private readonly IRelationshipIdentifier _relationshipIdentifier;
    private readonly ISqlCommandExpressionProvider _expressionProvider;
    private readonly DataContextConfigurationBase<AdoDataService> _configuration;

    public SqlDbCrudsFactory(IRelationshipIdentifier relationshipIdentifier,
                             ISqlCommandExpressionProvider expressionProvider,
                             DataContextConfigurationBase<AdoDataService> configuration)
    {
        _relationshipIdentifier = relationshipIdentifier;
        _expressionProvider = expressionProvider;
        _configuration = configuration;
    }

    public SqlDbCrud SqlCrudFor<TEntity>(SqlConnection connection)
        where TEntity: DomainModelBase => _relationshipIdentifier.IdentifyRelationship<TEntity>() switch
    {
        ObjectRelationship.Single => new SingleSqlDbCrud(connection, _expressionProvider, _configuration),
        ObjectRelationship.ToAnother => new ToAnotherSqlDbCrud(connection, _expressionProvider, _configuration),
        ObjectRelationship.ToMany => new ToManySqLDbCrud(connection, _expressionProvider, _configuration),
        ObjectRelationship.Composite => new CompositeSqlDbCrud(connection, _expressionProvider, _configuration),
        _ => throw new ArgumentException()
    };
}