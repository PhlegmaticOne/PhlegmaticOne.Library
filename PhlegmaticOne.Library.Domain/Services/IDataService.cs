using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Domain.Services;

public interface IDataService
{
    Task<int?> AddAsync<TEntity>(TEntity entity) where TEntity : DomainModelBase;
    Task<int> DeleteAsync<TEntity>(int id) where TEntity : DomainModelBase;
    Task UpdateAsync<TEntity>(int id, TEntity newEntity) where TEntity : DomainModelBase;
    Task<TEntity?> GetLazyAsync<TEntity>(int id) where TEntity : DomainModelBase;
    Task<TEntity?> GetFullAsync<TEntity>(int id) where TEntity : DomainModelBase;
    Task<int> GetIdOfExisting<TEntity>(TEntity entity) where TEntity : DomainModelBase;
}