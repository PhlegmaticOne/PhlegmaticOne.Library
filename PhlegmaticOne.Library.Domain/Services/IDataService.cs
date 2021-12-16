using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Domain.Services;
/// <summary>
/// Represents contract for domain entities processing
/// </summary>
public interface IDataService
{
    /// <summary>
    /// Adds new entity in storage asynchronously
    /// </summary>
    /// <returns>Id of added entity</returns>
    Task<int?> AddAsync<TEntity>(TEntity entity) where TEntity : DomainModelBase;
    /// <summary>
    /// Deletes entity from storage asynchronously
    /// </summary>
    /// <returns>Deleted count of entities</returns>
    Task<int> DeleteAsync<TEntity>(int id) where TEntity : DomainModelBase;
    /// <summary>
    /// Updates entity in storage asynchronously
    /// </summary>
    /// <param name="newEntity">New entity for updating old entity</param>
    /// <param name="id">Id of old entity</param>
    Task UpdateAsync<TEntity>(int id, TEntity newEntity) where TEntity : DomainModelBase;
    /// <summary>
    /// Gets entity in lazy way. Doesn't contain related objects entities
    /// </summary>
    /// <param name="id">Id of searching entity</param>
    /// <returns>Founded entity</returns>
    Task<TEntity?> GetLazyAsync<TEntity>(int id) where TEntity : DomainModelBase;
    /// <summary>
    /// Gets entity in full way. Contains related objects entities
    /// </summary>
    /// <param name="id">Id of searching entity</param>
    /// <returns>Founded entity</returns>
    Task<TEntity?> GetFullAsync<TEntity>(int id) where TEntity : DomainModelBase;
}