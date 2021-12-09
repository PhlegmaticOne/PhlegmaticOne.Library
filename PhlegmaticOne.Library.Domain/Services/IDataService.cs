using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Domain.Services;

public interface IDataService
{
    Task<bool> AddAsync(DomainModelBase entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, DomainModelBase newEntity);
    Task<DomainModelBase> GetLazyAsync(int id);
    Task<DomainModelBase> GetFullAsync(int id);
    Task<int> GetIdOfExisting(DomainModelBase entity);
}