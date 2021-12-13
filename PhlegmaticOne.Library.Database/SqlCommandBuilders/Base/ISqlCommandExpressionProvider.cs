using System.ComponentModel;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;

public interface ISqlCommandExpressionProvider
{
    string? SelectIdExpression(DomainModelBase? entity);
    string SelectLazyByIdExpression<TEntity>(int id) where TEntity: DomainModelBase;
    string SelectFromManyToManyTable(int primaryId, string tableName, string primaryColumnName, string foreignColumnName);
}