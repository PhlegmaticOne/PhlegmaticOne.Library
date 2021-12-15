using System.Reflection;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Configuration;

public class AdoDataContextConfiguration : DataContextConfigurationBase<AdoDataService>
{
    public override IDictionary<Type, string> TableNames => new Dictionary<Type, string>()
    {
        { typeof(Book), "BooksAuthors" },
        { typeof(Author), "BooksAuthors" }
    };
    public override string IdentificationPropertyName => "Id";
    public override TableNamesConfiguringType TableNamesConfiguringType => TableNamesConfiguringType.AddSToTypeName;
    public override string ForeignPropertyNameFor(Type propertyType) => propertyType.Name + IdentificationPropertyName;
    public override Type ToManyCollectionType => typeof(IEnumerable<DomainModelBase>);
    public override DomainModelBase ManyToManyAddingEntity => new Book();
    public override ManyToManyAddingType ManyToManyAddingType => ManyToManyAddingType.ForeignPropertiesMustExist;
    public override OneToManyAddingType OneToManyAddingType => OneToManyAddingType.ForeignPropertiesMustExist;
}