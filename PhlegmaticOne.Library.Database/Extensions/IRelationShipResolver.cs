using System.Collections;
using System.Reflection;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Extensions;

public interface IRelationShipResolver
{
    IEnumerable<PropertyInfo> ToManyProperties(DomainModelBase model);
    IEnumerable<PropertyInfo> ToAnotherProperties(DomainModelBase model);
    IEnumerable CastTo(List<DomainModelBase> entities, Type castType);
}