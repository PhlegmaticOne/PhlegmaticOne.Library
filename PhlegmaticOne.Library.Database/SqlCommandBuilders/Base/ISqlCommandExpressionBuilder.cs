using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;

public interface ISqlCommandExpressionBuilder
{
    string InsertExpression(DomainModelBase entity);
    string SelectIdExpression(DomainModelBase entity);
}