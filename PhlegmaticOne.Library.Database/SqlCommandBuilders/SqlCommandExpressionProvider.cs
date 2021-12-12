using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders;

public class SqlCommandExpressionProvider : ISqlCommandExpressionProvider
{
    public string SelectIdExpression<TEntity>(TEntity entity) where TEntity: DomainModelBase
    {
        var entityType = typeof(TEntity);
        var properties = entity.GetType().GetProperties()
            .WithoutAppearance<DomainModelBase>(p => p.Name != "Id")
            .Select(p =>
            {
                var obj = p.GetValue(entity);
                switch (obj)
                {
                    case string or DateTime: return $"{p.Name}='{obj}'";
                    default: return $"{p.Name}={obj}";
                }
            });
        return $"SELECT Id FROM {entityType.Name}s WHERE {string.Join(" AND ", properties)}";
    }
}