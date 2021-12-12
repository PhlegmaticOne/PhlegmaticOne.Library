using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Configuration;

public class AdoDataContextConfiguration : DataContextConfigurationBase<AdoDataService>
{
    public override DomainModelBase ManyToManyAddingEntity => new Book();
    public override ManyToManyAddingType ManyToManyAddingType => ManyToManyAddingType.ForeignPropertiesMustExist;
    public override OneToManyAddingType OneToManyAddingType => OneToManyAddingType.ForeignPropertiesMustExist;
}