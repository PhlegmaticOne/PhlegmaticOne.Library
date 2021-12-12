using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Extensions;

public static class SqlCommandExtensions
{
    public static DataTable First(this DataTableCollection collection) => collection[0];
    public static IEnumerable<PropertyInfo> WithoutAppearance<T>(this PropertyInfo[] propertyInfos) =>
        propertyInfos.Where(p =>
            p.PropertyType.IsAssignableTo(typeof(T)) == false &&
            p.PropertyType.IsAssignableTo(typeof(IEnumerable<T>)) == false);
    public static IEnumerable<PropertyInfo> WithoutAppearance<T>(this PropertyInfo[] propertyInfos, Func<PropertyInfo, bool> additionalPredicate) =>
        propertyInfos.Where(p =>
            p.PropertyType.IsAssignableTo(typeof(T)) == false &&
            p.PropertyType.IsAssignableTo(typeof(IEnumerable<T>)) == false).Where(additionalPredicate);
    public static DataRow ParametrizeWith(this DataRow dataRow, IDictionary<string, object> properties)
    {
        foreach (var property in properties)
        {
            dataRow[property.Key] = property.Value;
        }
        return dataRow;
    }
}