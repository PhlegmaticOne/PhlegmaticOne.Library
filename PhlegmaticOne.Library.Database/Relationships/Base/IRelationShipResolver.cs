using PhlegmaticOne.Library.Domain.Models;
using System.Collections;
using System.Reflection;

namespace PhlegmaticOne.Library.Database.Relationships.Base;
/// <summary>
/// Represents contract for processing entities with their relationships 
/// </summary>
public interface IRelationShipResolver
{
    /// <summary>
    /// Retrieves properties which has many to many relationship from entity
    /// </summary>
    /// <param name="model"></param>
    IEnumerable<PropertyInfo> ToManyToManyProperties(DomainModelBase model);
    /// <summary>
    /// Retrieves properties which are related to another entities
    /// </summary>
    /// <param name="model"></param>
    IEnumerable<PropertyInfo> ToToAnotherProperties(DomainModelBase model);
    /// <summary>
    /// Casts collection of elements to specified type
    /// </summary>
    IEnumerable CastTo(List<DomainModelBase> entities, Type castType);
    /// <summary>
    /// Retrieves properties which are doesn't relate to any entities
    /// </summary>
    public IEnumerable<PropertyInfo> ToNoRelationshipProperties(IEnumerable<PropertyInfo> properties);
}