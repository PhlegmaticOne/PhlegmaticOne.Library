using PhlegmaticOne.Library.Domain.Services;

namespace PhlegmaticOne.Library.Database.Configuration.Base;
/// <summary>
/// Represents base database configuration instance  
/// </summary>
public abstract class DataContextConfigurationBase<TContext> where TContext : IDataService
{
    /// <summary>
    /// Many to many table names with accessing by domain entities types
    /// </summary>
    public abstract IDictionary<Type, string> TableNames { get; }
    /// <summary>
    /// Identification property name of domain entities
    /// </summary>
    public abstract string IdPropertyName { get; }
    /// <summary>
    /// Identifies which type is many to many relation type
    /// </summary>
    public abstract Type ManyToManyCollectionType { get; }
    /// <summary>
    /// Identifies which type is to another type
    /// </summary>
    public abstract Type ToToAnotherEntityType { get; }
    /// <summary>
    /// Identifies policy for adding entities with many to many relationship
    /// </summary>
    public abstract ManyToManyAddingType ManyToManyAddingType { get; }
    /// <summary>
    /// Identifies policy for adding entities with one to many relationship
    /// </summary>
    public abstract OneToManyAddingType OneToManyAddingType { get; }
    /// <summary>
    /// Identifies policy for updating entities with many to many relationship
    /// </summary>
    public abstract OneToManyUpdatingType OneToManyUpdatingType { get; }
    /// <summary>
    /// Identifies policy for updating entities with one to many relationship
    /// </summary>
    public abstract ManyToManyUpdatingType ManyToManyUpdatingType { get; }
    /// <summary>
    /// Identifies datetime format used in application
    /// </summary>
    public abstract string DateTimeFormat { get; }
    /// <summary>
    /// Identifies policy for configuring table names by application types
    /// </summary>
    public abstract TableNamesConfiguringType TableNamesConfiguringType { get; }
    /// <summary>
    /// Converts type to table name
    /// </summary>
    public virtual string ToTableName(Type type) => TableNamesConfiguringType switch
    {
        TableNamesConfiguringType.AddSToTypeName => type.Name + "s",
        _ => type.Name
    };
    /// <summary>
    /// Converts type to foreign entity id name 
    /// </summary>
    public virtual string ForeignPropertyNameFor(Type propertyType) => propertyType.Name + IdPropertyName;
}