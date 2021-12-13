using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Relationships;

public class RelationshipIdentifier : IRelationshipIdentifier
{
    public ObjectRelationship IdentifyRelationship<TEntity>() where TEntity : DomainModelBase => IdentifyRelationship(typeof(TEntity));
    public ObjectRelationship IdentifyRelationship(DomainModelBase entity) => IdentifyRelationship(entity.GetType());
    public ObjectRelationship IdentifyRelationship(Type entityType)
    {
        var properties = entityType.GetProperties();
        if (properties.Any(p => p.PropertyType.IsAssignableTo(typeof(IEnumerable<DomainModelBase>))))
        {
            return properties.Any(p => p.PropertyType.IsAssignableTo(typeof(DomainModelBase))) ?
                ObjectRelationship.Composite : ObjectRelationship.ToMany;
        }
        return properties.Any(p => p.PropertyType.IsAssignableTo(typeof(DomainModelBase))) ?
            ObjectRelationship.ToAnother : ObjectRelationship.Single;
    }
}