using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Relationships.Base;

public interface IRelationshipIdentifier
{
    ObjectRelationship IdentifyRelationship<TEntity>() where TEntity: DomainModelBase;
}

public enum ObjectRelationship
{
    Single,
    ToAnother,
    ToMany,
    Composite
}