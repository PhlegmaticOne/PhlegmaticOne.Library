using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.Factory;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Repository;
/// <summary>
/// Represents sql repository instance
/// </summary>
public class SqlRepository : IRepository
{
    private readonly IConnectionStringGetter _connectionName;
    /// <summary>
    /// Initializes new SqlRepository instance
    /// </summary>
    /// <param name="connection">Connection string getter</param>
    public SqlRepository(IConnectionStringGetter connection) => _connectionName = connection;
    public async Task<IEnumerable<TEntity>> ReadAll<TEntity>() where TEntity : DomainModelBase
    {
        await using var sqlService = await AdoDataServiceFactory.DefaultInstanceAsync(_connectionName);
        var ids = await sqlService
            .ExecuteCommand($"SELECT {sqlService.DataContextConfiguration.IdPropertyName} " +
                            $"FROM {sqlService.DataContextConfiguration.ToTableName(typeof(TEntity))}");
        var result = new List<TEntity>();
        foreach (int id in ids)
        {
            result.Add(await sqlService.GetFullAsync<TEntity>(id));
        }
        return result;
    }
}