using PhlegmaticOne.Library.Domain.Models;
using PhlegmaticOne.Library.Domain.Services;

namespace PhlegmaticOne.Library.Database.Configuration.Base;

public abstract class DataContextConfigurationBase<TContext> where TContext : IDataService
{
    public abstract IDictionary<Type, string> TableNames { get; }
    public abstract string IdentificationPropertyName { get; }
    public abstract TableNamesConfiguringType TableNamesConfiguringType { get; }
    public abstract DomainModelBase ManyToManyAddingEntity { get; }
    public abstract ManyToManyAddingType ManyToManyAddingType { get; }
    public abstract OneToManyAddingType OneToManyAddingType { get; }
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