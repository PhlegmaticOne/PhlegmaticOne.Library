using PhlegmaticOne.Library.Database.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database;
/// <summary>
/// Helper class for operating with sql adapter
/// </summary>
internal static class SqlHelper
{
    /// <summary>
    /// Fill table with data from request and returns it 
    /// </summary>
    /// <param name="connection">Sql connection</param>
    /// <param name="expression">Sql expression</param>
    internal static DataTable ToFilledDataTable(SqlConnection connection, string expression)
    {
        var sqlAdapter = new SqlDataAdapter(expression, connection);
        var dataSet = new DataSet();
        sqlAdapter.Fill(dataSet);
        return dataSet.Tables.First();
    }
    /// <summary>
    /// Fill table with data from request and returns it 
    /// </summary>
    /// <param name="connection">Sql connection</param>
    /// <param name="expression">Sql expression</param>
    /// <param name="sqlAdapter">Returns sql adapter</param>
    /// <param name="dataSet">Returns data set</param>
    internal static DataTable ToFilledDataTable(SqlConnection connection, string expression, out SqlDataAdapter sqlAdapter, out DataSet dataSet)
    {
        sqlAdapter = new SqlDataAdapter(expression, connection);
        dataSet = new DataSet();
        sqlAdapter.Fill(dataSet);
        return dataSet.Tables.First();
    }
    /// <summary>
    /// Saves changes in data set
    /// </summary>
    /// <param name="sqlAdapter">Sql adapter</param>
    /// <param name="dataSet">Configured data set</param>
    internal static void SaveChanges(SqlDataAdapter sqlAdapter, DataSet dataSet)
    {
        new SqlCommandBuilder(sqlAdapter);
        sqlAdapter.Update(dataSet);
    }
    /// <summary>
    /// Executes reading command and returns first number from read data
    /// </summary>
    /// <param name="commandText">Sql command</param>
    /// <param name="connection">Sql connection</param>
    internal static async Task<int?> ExecuteReadingOneNumberCommand(string? commandText, SqlConnection connection)
    {
        if (commandText is null) return null;
        await using var command = new SqlCommand(commandText, connection);
        await using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return reader.GetInt32(0);
    }
    /// <summary>
    /// Executes void sql command asynchronously
    /// </summary>
    /// <param name="commandText">Sql command</param>
    /// <param name="connection">Sql connection</param>
    internal static async Task ExecuteVoidCommand(string? commandText, SqlConnection connection)
    {
        await using var command = new SqlCommand(commandText, connection);
        await command.ExecuteNonQueryAsync();
    }
}