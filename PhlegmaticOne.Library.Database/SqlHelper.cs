using System.Data;
using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Extensions;

namespace PhlegmaticOne.Library.Database;

internal static class SqlHelper
{
    internal static DataTable ToFilledDataTable(SqlConnection connection, string expression)
    {
        var sqlAdapter = new SqlDataAdapter(expression, connection);
        var dataSet = new DataSet();
        sqlAdapter.Fill(dataSet);
        return dataSet.Tables.First();
    }
}