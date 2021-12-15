using PhlegmaticOne.Library.Database.Extensions;
using System.Data;
using System.Data.SqlClient;

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
    internal static DataTable ToFilledDataTable(SqlConnection connection, string expression, out SqlDataAdapter sqlAdapter, out DataSet dataSet)
    {
        sqlAdapter = new SqlDataAdapter(expression, connection); 
        dataSet = new DataSet();
        sqlAdapter.Fill(dataSet);
        return dataSet.Tables.First();
    }
    internal static void SaveChanges(SqlDataAdapter sqlAdapter, DataSet dataSet)
    {
        new SqlCommandBuilder(sqlAdapter);
        sqlAdapter.Update(dataSet);
    }
    internal static async Task<int?> ExecuteReadingOneNumberCommand(string? commandText, SqlConnection connection)
    {
        if (commandText is null) return null;
        await using var command = new SqlCommand(commandText, connection);
        await using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return reader.GetInt32(0);
    }
}