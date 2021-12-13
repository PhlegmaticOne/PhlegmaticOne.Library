using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Relationships.Base;

public interface IRelationshipIdentifier
{
    ObjectRelationship IdentifyRelationship<TEntity>() where TEntity : DomainModelBase;
    ObjectRelationship IdentifyRelationship(DomainModelBase entity);
    ObjectRelationship IdentifyRelationship(Type entityType);
}

public enum ObjectRelationship
{
    Single,
    ToAnother,
    ToMany,
    Composite
}