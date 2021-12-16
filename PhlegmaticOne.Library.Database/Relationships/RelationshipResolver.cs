using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Collections;
using System.Reflection;

namespace PhlegmaticOne.Library.Database.Relationships;
/// <summary>
/// Represents instance for processing entities with their relationships 
/// </summary>
public class RelationshipResolver : IRelationShipResolver
{
    private readonly DataContextConfigurationBase<AdoDataService> _configuration;
    /// <summary>
    /// Initializes new RelationshipResolver instance
    /// </summary>
    /// <param name="configuration">Database configuration</param>
    public RelationshipResolver(DataContextConfigurationBase<AdoDataService> configuration) =>
        _configuration = configuration;
    public IEnumerable<PropertyInfo> ToManyToManyProperties(DomainModelBase model) =>
        model.GetType().GetProperties()
            .Where(p => p.PropertyType.IsAssignableTo(_configuration.ManyToManyCollectionType));

    public IEnumerable<PropertyInfo> ToToAnotherProperties(DomainModelBase model) =>
        model.GetType().GetProperties()
            .Where(p => p.PropertyType.IsAssignableTo(_configuration.ToToAnotherEntityType));

    public IEnumerable<PropertyInfo> ToNoRelationshipProperties(IEnumerable<PropertyInfo> properties) =>
        properties.Where(p =>
            p.PropertyType.IsAssignableTo(_configuration.ToToAnotherEntityType) == false &&
            p.PropertyType.IsAssignableTo(_configuration.ManyToManyCollectionType) == false);

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