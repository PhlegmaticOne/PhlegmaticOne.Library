using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;

namespace PhlegmaticOne.Library.Database.Factory;

public class AdoDataServiceFactory
{
    private static AdoDataService? _adoDataService;
    private static readonly object _lock = new();
    private AdoDataServiceFactory() { }
    public static AdoDataService GetInstance(IConnectionStringGetter connectionStringGetter,
                                             ISqlCommandExpressionBuilder sqlCommandExpressionBuilder)
    {
        lock (_lock) return _adoDataService ??= new AdoDataService(connectionStringGetter, sqlCommandExpressionBuilder);
    }
}