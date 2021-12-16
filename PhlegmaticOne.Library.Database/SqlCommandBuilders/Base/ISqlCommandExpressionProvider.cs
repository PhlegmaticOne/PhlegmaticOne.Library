using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
/// <summary>
/// Represents contract for building sql commands
/// </summary>
public interface ISqlCommandExpressionProvider
{
    /// <summary>
    /// Builds command for selecting id of entity in database by its application instance
    /// </summary>
    /// <param name="entity">Application instance</param>
    string? SelectIdExpression(DomainModelBase? entity);
    /// <summary>
    /// Builds command for lazy selecting from database
    /// </summary>
    /// <param name="id">Id of row in database</param>
    /// <param name="type">Entity type to select</param>
    string SelectLazyByIdExpression(int id, Type type);
    /// <summary>
    /// Builds command for selecting one of ids from many to many temp table
    /// </summary>
    /// <param name="primaryId">Id for searching record in table</param>
    /// <param name="tableName">Many to many table name</param>
    /// <param name="primaryColumnName">Column name with id to search</param>
    /// <param name="foreignColumnName">Other column name</param>
    string SelectFromManyToManyTable(int primaryId, string tableName, string primaryColumnName, string foreignColumnName);
    /// <summary>
    /// Builds delete expression
    /// </summary>
    /// <param name="id">Id of record in database</param>
    string DeleteExpression<TEntity>(int id) where TEntity : DomainModelBase;
    /// <summary>
    /// Builds command for getting id of last added record
    /// </summary>
    string GetLastIdFor<TEntity>() where TEntity : DomainModelBase;
    /// <summary>
    /// Builds command for empty selecting from table
    /// </summary>
    string EmptySelectFor<TEntity>() where TEntity : DomainModelBase;
    /// <summary>
    /// Builds command for empty selecting from table
    /// </summary>
    /// <param name="tableName">Table name</param>
    string EmptySelectFor(string tableName);
    /// <summary>
    /// Builds command for updating record in table with new entity
    /// </summary>
    /// <param name="oldId">Id of old existing record</param>
    /// <param name="entity">New entity</param>
    string UpdateExpression<TEntity>(int oldId, TEntity entity) where TEntity : DomainModelBase;
    /// <summary>
    /// Builds expression for deleting record in many to many table
    /// </summary>
    string DeleteFromManyToManyTableExpression(string tableName, string firstColumnName, string secondColumnName, int firstId, int secondId);
}