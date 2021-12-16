using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.SqlCommandBuilders;

public class SqlCommandExpressionProvider : ISqlCommandExpressionProvider
{
    private readonly DataContextConfigurationBase<AdoDataService> _configuration;
    private readonly IRelationShipResolver _relationShipResolver;
    public SqlCommandExpressionProvider(DataContextConfigurationBase<AdoDataService> configuration,
                                        IRelationShipResolver relationShipResolver)
    {
        _configuration = configuration;
        _relationShipResolver = relationShipResolver;
    }

    public string? SelectIdExpression(DomainModelBase? entity)
    {
        if (entity is null) return null;
        var entityType = entity.GetType();
        var properties = _relationShipResolver.ToNoRelationshipProperties(entityType.GetProperties())
            .Where(p => p.Name.Contains(_configuration.IdPropertyName) == false)
            .Select(p =>
            {
                var obj = p.GetValue(entity);
                return obj switch
                {
                    string => $"{p.Name}='{obj}'",
                    DateTime dt => $"{p.Name}='{dt.ToString(_configuration.DateTimeFormat)}'",
                    _ => $"{p.Name}={obj}"
                };
            });
        return $"SELECT {_configuration.IdPropertyName} FROM {_configuration.ToTableName(entityType)} " +
               $"WHERE {string.Join(" AND ", properties)}";
    }
    public string SelectLazyByIdExpression(int id, Type type) => $"SELECT * FROM {_configuration.ToTableName(type)} " +
                                                                 $"WHERE {_configuration.IdPropertyName}={id}";
    public string SelectFromManyToManyTable(int primaryId, string tableName, string primaryColumnName, string foreignColumnName) =>
        $"SELECT {foreignColumnName} FROM {tableName} WHERE {primaryColumnName}={primaryId}";
    public string DeleteExpression<TEntity>(int id) where TEntity : DomainModelBase =>
        $"DELETE FROM {_configuration.ToTableName(typeof(TEntity))} WHERE {_configuration.IdPropertyName}={id}";
    public string GetLastIdFor<TEntity>() where TEntity : DomainModelBase =>
        $"SELECT MAX({_configuration.IdPropertyName}) FROM {_configuration.ToTableName(typeof(TEntity))}";
    public string EmptySelectFor<TEntity>() where TEntity : DomainModelBase =>
        $"SELECT * FROM {_configuration.ToTableName(typeof(TEntity))} WHERE {_configuration.IdPropertyName}=-1";

    public string EmptySelectFor(string tableName) => $"SELECT TOP 0 * FROM {tableName}";
    public string UpdateExpression<TEntity>(int oldId, TEntity entity) where TEntity : DomainModelBase
    {
        var newProperties =
            _relationShipResolver.ToNoRelationshipProperties(typeof(TEntity).GetProperties())
                .Where(p => p.Name != _configuration.IdPropertyName)
                .Select(p =>
                {
                    var obj = p.GetValue(entity);
                    return obj switch
                    {
                        string => $"{p.Name}='{obj}'",
                        DateTime dt => $"{p.Name}='{dt.ToString(_configuration.DateTimeFormat)}'",
                        _ => $"{p.Name}={obj}"
                    };
                });
        return $"UPDATE {_configuration.ToTableName(typeof(TEntity))} SET {string.Join(", ", newProperties)} " +
               $"WHERE {_configuration.IdPropertyName}={oldId}";
    }

    public string DeleteFromManyToManyTableExpression(string tableName, string firstColumnName, string secondColumnName, int firstId, int secondId) =>
        $"DELETE FROM {tableName} WHERE {firstColumnName}={firstId} AND {secondColumnName}={secondId}";
}