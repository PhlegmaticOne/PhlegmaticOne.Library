using PhlegmaticOne.Library.Database.Configuration;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.CRUDs;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships;
using PhlegmaticOne.Library.Database.SqlCommandBuilders;

namespace PhlegmaticOne.Library.Database.Factory;

public class AdoDataServiceFactory
{
    private static Task<AdoDataService>? _adoDataService;
    private static readonly object _lock = new();
    private AdoDataServiceFactory() { }
    public static Task<AdoDataService> GetInstanceAsync(IConnectionStringGetter connectionStringGetter,
                                                        SqlDbCrudsFactory sqlDbCrudsFactory)
    {
        lock (_lock) return _adoDataService ??= AdoDataService.CreateInstanceAsync(connectionStringGetter, sqlDbCrudsFactory);
    }

    public static Task<AdoDataService> DefaultInstanceAsync(IConnectionStringGetter connectionStringGetter)
    {
        lock (_lock)
            return _adoDataService ??= AdoDataService.CreateInstanceAsync(connectionStringGetter,
                new SqlDbCrudsFactory(new RelationshipIdentifier(), new SqlCommandExpressionProvider(), new AdoDataContextConfiguration()));
    }
}