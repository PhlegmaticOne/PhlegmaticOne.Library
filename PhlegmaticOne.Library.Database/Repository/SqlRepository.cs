using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.Factory;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Repository;

public class SqlRepository : IRepository
{
    public async Task<IEnumerable<TEntity>> ReadAll<TEntity>() where TEntity: DomainModelBase
    {
        await using var sqlService = await AdoDataServiceFactory.DefaultInstanceAsync(DefaultConnectionStringGetter.LibraryConnectionStringGetter);
        var ids = await sqlService.ExecuteCommand($"SELECT Id FROM {typeof(TEntity).Name}");
        var result = new List<TEntity>();
        foreach (int id in ids)
        {
            result.Add(await sqlService.GetFullAsync<TEntity>(id));
        }
        return result;
    }
}