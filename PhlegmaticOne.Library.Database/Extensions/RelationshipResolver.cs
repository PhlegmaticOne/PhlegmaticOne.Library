using System.Collections;
using System.Reflection;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Extensions;

public class RelationshipResolver : IRelationShipResolver
{
    private readonly DataContextConfigurationBase<AdoDataService> _configuration;
    public RelationshipResolver(DataContextConfigurationBase<AdoDataService> configuration) => _configuration = configuration;

    public IEnumerable<PropertyInfo> ToManyProperties(DomainModelBase model) =>
        model.GetType().GetProperties()
            .Where(p => p.PropertyType.IsAssignableTo(_configuration.ToManyCollectionType));

    public IEnumerable<PropertyInfo> ToAnotherProperties(DomainModelBase model) =>
        model.GetType().GetProperties()
            .Where(p => p.PropertyType.IsAssignableTo(typeof(DomainModelBase)));

    public IEnumerable CastTo(List<DomainModelBase> entities, Type castType)
    {
        if (_configuration.TableNames.ContainsKey(castType))
        {
            return castType.Name switch
            {
                "Author" => entities.Cast<Author>().ToList(),
                "Book" => entities.Cast<Book>().ToList(),
                _ => entities
            };
        }

        throw new ArgumentException();
    }
}