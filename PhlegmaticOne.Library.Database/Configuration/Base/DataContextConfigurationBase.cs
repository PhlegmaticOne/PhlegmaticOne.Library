using PhlegmaticOne.Library.Domain.Services;

namespace PhlegmaticOne.Library.Database.Configuration.Base;

public abstract class DataContextConfigurationBase<TContext> where TContext : IDataService
{
    public abstract IDictionary<Type, string> TableNames { get; }
    public abstract string IdPropertyName { get; }
    public abstract Type ManyToManyCollectionType { get; }
    public abstract Type ToToAnotherEntityType { get; }
    public abstract ManyToManyAddingType ManyToManyAddingType { get; }
    public abstract OneToManyAddingType OneToManyAddingType { get; }
    public abstract OneToManyUpdatingType OneToManyUpdatingType { get; }
    public abstract ManyToManyUpdatingType ManyToManyUpdatingType { get; }
    public abstract string DateTimeFormat { get; }
    public abstract TableNamesConfiguringType TableNamesConfiguringType { get; }
    public virtual string ToTableName(Type type) => TableNamesConfiguringType switch
    {
        TableNamesConfiguringType.AddSToTypeName => type.Name + "s",
        _ => type.Name
    };
    public virtual string ForeignPropertyNameFor(Type propertyType) => propertyType.Name + IdPropertyName;
}

public enum ManyToManyAddingType
{
    ForeignPropertiesMustExist
}

public enum OneToManyAddingType
{
    ForeignPropertiesMustExist
}

public enum TableNamesConfiguringType
{
    AddSToTypeName
}

public enum OneToManyUpdatingType
{
    ForeignPropertiesMustExist
}

public enum ManyToManyUpdatingType
{
    AddDeleteRelatedEntitiesWhenChanged
}