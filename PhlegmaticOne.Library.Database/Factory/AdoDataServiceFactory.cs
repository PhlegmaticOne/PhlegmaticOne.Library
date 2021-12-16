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

    public static Task<AdoDataService> DefaultInstanceAsync(IConnectionStringGetter connectionStringGetter)
    {
        if (_adoDataService is null)
        {
            lock (_lock)
            {
                var configuration = new AdoDataContextConfiguration();
                var relationShopResolver = new RelationshipResolver(configuration);
                var relationShipIdentifier = new RelationshipIdentifier(configuration);
                var sqlCommandExpressionProvider =
                    new SqlCommandExpressionProvider(configuration, relationShopResolver);
                var sqlAddingFactory =
                    new SqlDbAddingFactory(relationShipIdentifier, sqlCommandExpressionProvider, configuration, relationShopResolver);
                _adoDataService = AdoDataService
                    .CreateInstanceAsync(connectionStringGetter, sqlAddingFactory, relationShipIdentifier,
                        sqlCommandExpressionProvider);
            }
        }
        return _adoDataService;
    }
}