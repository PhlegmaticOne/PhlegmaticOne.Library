using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Relationships.Base;
/// <summary>
/// Represents contract for identifying relation ship of entities
/// </summary>
public interface IRelationshipIdentifier
{
    /// <summary>
    /// Identify relationship by generic parameter
    /// </summary>
    ObjectRelationship IdentifyRelationship<TEntity>() where TEntity : DomainModelBase;
    /// <summary>
    /// Identify relationship by existing entity
    /// </summary>
    ObjectRelationship IdentifyRelationship(DomainModelBase entity);
    /// <summary>
    /// Identify relationship by entity type
    /// </summary>
    ObjectRelationship IdentifyRelationship(Type entityType);
}