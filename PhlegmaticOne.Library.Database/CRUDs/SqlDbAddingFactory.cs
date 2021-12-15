using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
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

    public SqlDbAddingFactory(IRelationshipIdentifier relationshipIdentifier,
                             ISqlCommandExpressionProvider expressionProvider,
                             DataContextConfigurationBase<AdoDataService> configuration)
    {
        _relationshipIdentifier = relationshipIdentifier;
        _expressionProvider = expressionProvider;
        _configuration = configuration;
    }

    public SqlDbAdding SqlCrudFor<TEntity>(SqlConnection connection)
        where TEntity : DomainModelBase => _relationshipIdentifier.IdentifyRelationship<TEntity>() switch
        {
            ObjectRelationship.Single => new SingleSqlDbAdding(connection, _expressionProvider, _configuration),
            ObjectRelationship.ToAnother => new ToAnotherSqlDbAdding(connection, _expressionProvider, _configuration),
            ObjectRelationship.ToMany => new ToManySqLDbAdding(connection, _expressionProvider, _configuration),
            ObjectRelationship.Composite => new CompositeSqlDbAdding(connection, _expressionProvider, _configuration),
            _ => throw new ArgumentException()
        };
    public SqlDbAdding SqlCrudFor(DomainModelBase entity, SqlConnection connection) => _relationshipIdentifier.IdentifyRelationship(entity) switch
    {
        ObjectRelationship.Single => new SingleSqlDbAdding(connection, _expressionProvider, _configuration),
        ObjectRelationship.ToAnother => new ToAnotherSqlDbAdding(connection, _expressionProvider, _configuration),
        ObjectRelationship.ToMany => new ToManySqLDbAdding(connection, _expressionProvider, _configuration),
        ObjectRelationship.Composite => new CompositeSqlDbAdding(connection, _expressionProvider, _configuration),
        _ => throw new ArgumentException()
    };
}