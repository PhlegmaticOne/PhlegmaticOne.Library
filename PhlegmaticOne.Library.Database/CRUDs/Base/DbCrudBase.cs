﻿using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.CRUDs.Base;

public abstract class DbCrudBase
{
    public abstract Task<int?> AddAsync<TEntity>(TEntity entity) where TEntity : DomainModelBase;
    public abstract void Update<TEntity>(int id, TEntity entity) where TEntity : DomainModelBase;
    public abstract void Delete<TEntity>(int id) where TEntity : DomainModelBase;
    public abstract Task<TEntity> GetLazy<TEntity>(int id) where TEntity : DomainModelBase;
    public abstract Task<TEntity> GetFull<TEntity>(int id) where TEntity : DomainModelBase;
}