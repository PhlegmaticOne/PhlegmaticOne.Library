using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Repository;

public interface IRepository
{
    Task<IEnumerable<TEntity>> ReadAll<TEntity>() where TEntity: DomainModelBase;
}