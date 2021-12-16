namespace PhlegmaticOne.Library.Database;
/// <summary>
/// Help constants for operating with database
/// </summary>
public static class SqlCommandsPatterns
{
    /// <summary>
    /// Expression for deleting all data from all tables
    /// </summary>
    public const string AllDeletingStatements =
        "SELECT CONCAT('DELETE FROM ', TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES " +
        "WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME<>'sysdiagrams'";
}