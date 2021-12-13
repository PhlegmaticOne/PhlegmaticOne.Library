using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders;

public class SqlCommandExpressionProvider : ISqlCommandExpressionProvider
{
    public string? SelectIdExpression(DomainModelBase? entity)
    {
        if (entity is null) return null;
        var entityType = entity.GetType();
        var properties = entity.GetType().GetProperties()
            .WithoutAppearance<DomainModelBase>(p => p.Name.Contains("Id") == false)
            .Select(p =>
            {
                var obj = p.GetValue(entity);
                return obj switch
                {
                    string => $"{p.Name}='{obj}'",
                    DateTime dt => $"{p.Name}='{dt:yyyy-MM-dd}'",
                    _ => $"{p.Name}={obj}"
                };
            });
        return $"SELECT Id FROM {entityType.Name}s WHERE {string.Join(" AND ", properties)}";
    }

    public string SelectLazyByIdExpression<TEntity>(int id) where TEntity : DomainModelBase => 
        $"SELECT * FROM {typeof(TEntity).Name}s WHERE Id={id}";

    public string SelectFromManyToManyTable(int primaryId, string tableName, string primaryColumnName, string foreignColumnName)
    {
        return $"SELECT {foreignColumnName} FROM {tableName} WHERE {primaryColumnName}={primaryId}";
    }
}