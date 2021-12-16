namespace PhlegmaticOne.Library.Database.Connection;
/// <summary>
/// Represents instance for getting connection string from application variables
/// </summary>
public class DefaultConnectionStringGetter : IConnectionStringGetter
{
    /// <summary>
    /// Returns connection string for librarydatabase
    /// </summary>
    public static DefaultConnectionStringGetter LibraryConnectionStringGetter =>
        new(@"(localdb)\MSSQLLocalDB", "LibraryDataBase");
    private readonly string _serverName;
    private readonly string _dataBaseName;
    /// <summary>
    /// Initializes new DefaultConnectionStringGetter instance
    /// </summary>
    /// <param name="serverName">Server name</param>
    /// <param name="dataBaseName">Data base name</param>
    /// <exception cref="ArgumentNullException"></exception>
    public DefaultConnectionStringGetter(string serverName, string dataBaseName)
    {
        _serverName = serverName ?? throw new ArgumentNullException(nameof(serverName));
        _dataBaseName = dataBaseName ?? throw new ArgumentNullException(nameof(dataBaseName));
    }
    public string GetConnectionString() => $@"Data Source={_serverName};Initial Catalog={_dataBaseName};Integrated Security=True";
}