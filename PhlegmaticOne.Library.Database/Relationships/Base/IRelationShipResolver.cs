using PhlegmaticOne.Library.Domain.Models;
using System.Collections;
using System.Reflection;

namespace PhlegmaticOne.Library.Database.Relationships.Base;

public interface IRelationShipResolver
{
    IEnumerable<PropertyInfo> ToManyToManyProperties(DomainModelBase model);
    IEnumerable<PropertyInfo> ToToAnotherProperties(DomainModelBase model);
    IEnumerable CastTo(List<DomainModelBase> entities, Type castType);
    public IEnumerable<PropertyInfo> ToNoRelationshipProperties(IEnumerable<PropertyInfo> properties);
}