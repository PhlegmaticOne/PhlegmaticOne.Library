namespace PhlegmaticOne.Library.Database.Connection;

public class DefaultConnectionStringGetter : IConnectionStringGetter
{
    public static DefaultConnectionStringGetter LibraryConnectionStringGetter =>
        new(@"(localdb)\MSSQLLocalDB", "LibraryDataBase");
    private readonly string _serverName;
    private readonly string _dataBaseName;
    public DefaultConnectionStringGetter(string serverName, string dataBaseName)
    {
        _serverName = serverName ?? throw new ArgumentNullException(nameof(serverName));
        _dataBaseName = dataBaseName ?? throw new ArgumentNullException(nameof(dataBaseName));
    }
    public string GetConnectionString() => $@"Data Source={_serverName};Initial Catalog={_dataBaseName};Integrated Security=True";
}

