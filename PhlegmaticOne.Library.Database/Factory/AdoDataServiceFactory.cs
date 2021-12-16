using PhlegmaticOne.Library.Database.Configuration;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.CRUDs;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships;
using PhlegmaticOne.Library.Database.SqlCommandBuilders;

namespace PhlegmaticOne.Library.Database.Factory;
/// <summary>
/// Represents singleton factory for database context 
/// </summary>
public class AdoDataServiceFactory
{
    private static Task<AdoDataService>? _adoDataService;
    private static readonly object _lock = new();
    private AdoDataServiceFactory() { }
    /// <summary>
    /// Gets default database context
    /// </summary>
    /// <param name="connectionStringGetter">Connection string getter</param>
    public static Task<AdoDataService> DefaultInstanceAsync(IConnectionStringGetter connectionStringGetter)
    {
        if (_adoDataService is not null) return _adoDataService;
        lock (_lock)
        {
            var configuration = new AdoDataContextConfiguration();
            var relationShipResolver = new RelationshipResolver(configuration);
            var relationShipIdentifier = new RelationshipIdentifier(configuration);
            var sqlCommandExpressionProvider =
                new SqlCommandExpressionProvider(configuration, relationShipResolver);
            var sqlAddingFactory =
                new SqlDbAddUpdateFactory(relationShipIdentifier, sqlCommandExpressionProvider, configuration, relationShipResolver);
            _adoDataService = AdoDataService
                .CreateInstanceAsync(connectionStringGetter, sqlAddingFactory,
                    configuration, relationShipIdentifier, sqlCommandExpressionProvider);
        }
        return _adoDataService;
    }
}