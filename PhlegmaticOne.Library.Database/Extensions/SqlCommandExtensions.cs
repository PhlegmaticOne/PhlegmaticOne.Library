using System.Data.SqlClient;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Extensions;

public static class SqlCommandExtensions
{
    public static SqlCommand Parametrize(this SqlCommand command, DomainModelBase entity)
    {
        foreach (var property in entity.GetType().GetProperties().Where(p => p.Name != "Id"))
        {
            if (property.PropertyType.IsAssignableTo(typeof(DomainModelBase)))
            {

            }
            command.Parameters.Add(new SqlParameter("@" + property.Name, property.GetValue(entity)));
        }
        return command;
    }
}