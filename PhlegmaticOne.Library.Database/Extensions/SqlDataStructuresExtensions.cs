using System.Data;

namespace PhlegmaticOne.Library.Database.Extensions;

public static class SqlDataStructuresExtensions
{
    public static DataTable First(this DataTableCollection collection) => collection[0];
    public static DataRow ParametrizeWith(this DataRow dataRow, IDictionary<string, object?> properties)
    {
        foreach (var property in properties)
        {
            dataRow[property.Key] = property.Value ?? DBNull.Value;
        }
        return dataRow;
    }
    public static void AddRowWith(this DataTable table, IDictionary<string, object?> properties) =>
        table.Rows.Add(table.NewRow().ParametrizeWith(properties));
}