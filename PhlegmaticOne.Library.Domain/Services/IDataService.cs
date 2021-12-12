﻿using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Domain.Services;

public interface IDataService
{
    Task<int> AddAsync<TEntity>(TEntity entity) where TEntity: DomainModelBase;
    Task<DeleteCommandResult<TEntity>> DeleteAsync<TEntity>(int id) where TEntity: DomainModelBase;
    Task<UpdateCommandResult<TEntity>> UpdateAsync<TEntity>(int id, TEntity newEntity) where TEntity: DomainModelBase;
    Task<GetCommandResult<TEntity>> GetLazyAsync<TEntity>(int id) where TEntity: DomainModelBase;
    Task<GetCommandResult<TEntity>> GetFullAsync<TEntity>(int id) where TEntity: DomainModelBase;
    Task<int> GetIdOfExisting<TEntity>(TEntity entity) where TEntity: DomainModelBase;
}