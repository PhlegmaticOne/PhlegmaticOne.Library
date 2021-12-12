using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;

public interface ISqlCommandExpressionProvider
{
    string? SelectIdExpression(DomainModelBase? entity);
}