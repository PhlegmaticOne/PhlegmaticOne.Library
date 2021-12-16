namespace PhlegmaticOne.Library.Database.Connection;
/// <summary>
/// Represents contract for getting connection strings
/// </summary>
public interface IConnectionStringGetter
{
    /// <summary>
    /// Gets connection string
    /// </summary>
    string GetConnectionString();
}