using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Repository;
/// <summary>
/// Represents contract for processing domain entities from different sources
/// </summary>
public interface IRepository
{
    /// <summary>
    /// Reads all data from source asynchronously
    /// </summary>
    /// <returns>Collection of read entities</returns>
    Task<IEnumerable<TEntity>> ReadAll<TEntity>() where TEntity : DomainModelBase;
}