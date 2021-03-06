using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Configuration;
/// <summary>
/// Represents default database configuration instance
/// </summary>
public class AdoDataContextConfiguration : DataContextConfigurationBase<AdoDataService>
{
    public override IDictionary<Type, string> TableNames => new Dictionary<Type, string>()
    {
        { typeof(Book), "BooksAuthors" },
        { typeof(Author), "BooksAuthors" }
    };
    public override string IdPropertyName => "Id";
    public override TableNamesConfiguringType TableNamesConfiguringType => TableNamesConfiguringType.AddSToTypeName;
    public override Type ManyToManyCollectionType => typeof(IEnumerable<DomainModelBase>);
    public override Type ToToAnotherEntityType => typeof(DomainModelBase);
    public override ManyToManyAddingType ManyToManyAddingType => ManyToManyAddingType.ForeignPropertiesMustExist;
    public override OneToManyAddingType OneToManyAddingType => OneToManyAddingType.ForeignPropertiesMustExist;
    public override OneToManyUpdatingType OneToManyUpdatingType => OneToManyUpdatingType.ForeignPropertiesMustExist;
    public override ManyToManyUpdatingType ManyToManyUpdatingType =>
        ManyToManyUpdatingType.AddDeleteRelatedEntitiesWhenChanged;
    public override string DateTimeFormat => "yyyy-MM-dd";
}