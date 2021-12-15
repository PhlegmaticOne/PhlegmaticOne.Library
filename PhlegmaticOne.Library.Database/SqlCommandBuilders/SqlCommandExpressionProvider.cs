using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
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
            .Where(p => p.Name.Contains(_configuration.IdentificationPropertyName) == false)
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
        return $"SELECT {_configuration.IdentificationPropertyName} FROM {_configuration.ToTableName(entityType)} " +
               $"WHERE {string.Join(" AND ", properties)}";
    }
    public string SelectLazyByIdExpression(int id, Type type) => $"SELECT * FROM {_configuration.ToTableName(type)} " +
                                                                 $"WHERE {_configuration.IdentificationPropertyName}={id}";
    public string SelectFromManyToManyTable(int primaryId, string tableName, string primaryColumnName, string foreignColumnName) =>
        $"SELECT {foreignColumnName} FROM {tableName} WHERE {primaryColumnName}={primaryId}";
    public string DeleteExpression<TEntity>(int id) where TEntity : DomainModelBase =>
        $"DELETE FROM {_configuration.ToTableName(typeof(TEntity))} WHERE {_configuration.IdentificationPropertyName}={id}";
    public string GetLastIdFor<TEntity>() where TEntity : DomainModelBase =>
        $"SELECT MAX({_configuration.IdentificationPropertyName}) FROM {_configuration.ToTableName(typeof(TEntity))}";
    public string EmptySelectFor<TEntity>() where TEntity : DomainModelBase =>
        $"SELECT * FROM {_configuration.ToTableName(typeof(TEntity))} WHERE {_configuration.IdentificationPropertyName}=-1";

    public string EmptySelectFor(string tableName) => $"SELECT TOP 0 * FROM {tableName}";
}