using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;

public interface ISqlCommandExpressionProvider
{
    string? SelectIdExpression(DomainModelBase? entity);
    string SelectLazyByIdExpression(int id, Type type);
    string SelectFromManyToManyTable(int primaryId, string tableName, string primaryColumnName, string foreignColumnName);
    string DeleteExpression<TEntity>(int id) where TEntity : DomainModelBase;
    string GetLastIdFor<TEntity>() where TEntity : DomainModelBase;
    string EmptySelectFor<TEntity>() where TEntity : DomainModelBase;
    string EmptySelectFor(string tableName);
    string UpdateExpression<TEntity>(int oldId, TEntity entity) where TEntity : DomainModelBase;
    string DeleteFromManyToManyTableExpression(string tableName, string firstColumnName, string secondColumnName, int firstId, int secondId);
}