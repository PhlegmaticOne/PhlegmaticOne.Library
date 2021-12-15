namespace PhlegmaticOne.Library.Database;

public static class SqlCommandsPatterns
{
    public static string AllDeletingStatements() =>
        "SELECT CONCAT('DELETE FROM ', TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES " +
        "WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME<>'sysdiagrams'";
}