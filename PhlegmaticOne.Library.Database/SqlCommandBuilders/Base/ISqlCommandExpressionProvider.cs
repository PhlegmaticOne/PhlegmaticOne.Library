using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;

public interface ISqlCommandExpressionProvider
{
    string SelectIdExpression<TEntity>(TEntity entity) where TEntity: DomainModelBase;
}