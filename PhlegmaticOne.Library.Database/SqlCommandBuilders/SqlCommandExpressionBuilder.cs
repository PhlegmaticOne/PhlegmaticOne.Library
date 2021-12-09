using System.Text;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders;

public class SqlCommandExpressionBuilder : ISqlCommandExpressionBuilder
{
    public string InsertExpression(DomainModelBase entity)
    {
        var entityType = entity.GetType();
        var propertyNames = entityType.GetProperties()
                                      .Where(p => p.Name != "Id" && p.PropertyType.IsAssignableTo(typeof(DomainModelBase)) == false)
                                      .Select(p => p.Name);
        var commandParameterNames = propertyNames.Select(x => "@" + x);
        return new StringBuilder().Append("INSERT INTO ").Append(entityType.Name + "s ")
                                  .Append("(" + string.Join(", ", propertyNames) + ")")
                                  .Append(" VALUES ").Append("(" + string.Join(", ", commandParameterNames) + ")")
                                  .ToString();
    }

    public string SelectIdExpression(DomainModelBase entity)
    {
        //SELECT Id FROM Genders WHERE Name=@Name AND 
        var entityType = entity.GetType();
        var propertyNames = entityType.GetProperties()
                                      .Where(p => p.Name != "Id" && p.PropertyType.IsAssignableTo(typeof(DomainModelBase)) == false)
                                      .Select(p => p.Name + "=@" + p.Name);
        return new StringBuilder().Append("SELECT Id FROM ").Append(entityType.Name + "s ")
                                  .Append("WHERE ").Append(string.Join(" AND ", propertyNames))
                                  .ToString();
    }
}