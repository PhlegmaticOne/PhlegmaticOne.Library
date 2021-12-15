using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;

public interface ISqlCommandExpressionProvider
{
    string? SelectIdExpression(DomainModelBase? entity);
    string SelectLazyByIdExpression(int id, Type type);
    string SelectFromManyToManyTable(int primaryId, string tableName, string primaryColumnName, string foreignColumnName);
    string DeleteExpression<TEntity>(int id) where TEntity : DomainModelBase;
    public string GetLastIdFor<TEntity>() where TEntity : DomainModelBase;
    public string EmptySelectFor<TEntity>() where TEntity : DomainModelBase;
    public string EmptySelectFor(string tableName);
}