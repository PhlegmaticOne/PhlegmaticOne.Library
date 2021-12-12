namespace PhlegmaticOne.Library.Database;

public static class SqlCommandsPatterns
{
    public static string EmptySelectFor<T>() => $"SELECT * FROM {typeof(T).Name}s WHERE Id=-1";
    public static string EmptySelectFor(string tableName) => $"SELECT TOP 0 * FROM {tableName}";
    public static string AllDeletingStatements() =>
        "SELECT CONCAT('DELETE FROM ', TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES " +
        "WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME<>'sysdiagrams'";
    public static string GetLastIdFor<TEntity>() => $"SELECT MAX(Id) FROM {typeof(TEntity).Name}s";
}