using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddUpdate.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Relationships.Base;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipAddUpdate;
/// <summary>
/// Represent instance for adding and updating one to many entities
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class ToAnotherSqlDbAdding<TEntity> : SqlDbAddUpdate<TEntity> where TEntity : DomainModelBase
{
    /// <summary>
    /// Initializes new ToAnotherSqlDbAdding instance
    /// </summary>
    public ToAnotherSqlDbAdding(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                                DataContextConfigurationBase<AdoDataService> configuration, IRelationShipResolver relationShipResolver) :
                                base(connection, expressionProvider, configuration, relationShipResolver)
    { }
    public override async Task<int?> AddAsync(TEntity entity)
    {
        return Configuration switch
        {
            { OneToManyAddingType: OneToManyAddingType.ForeignPropertiesMustExist } =>
                await Task.Factory.StartNew(async () =>
                {
                    var properties = typeof(TEntity).GetProperties();
                    foreach (var property in RelationShipResolver.ToToAnotherProperties(entity))
                    {
                        var relatedEntityId = await GetIdOfExisting(property.GetValue(entity) as DomainModelBase);
                        properties.First(p => p.Name == Configuration.ForeignPropertyNameFor(property.PropertyType))
                                  .SetValue(entity, relatedEntityId);
                    }
                    return entity;
                })
                .ContinueWith(async configuredEntityTask => await base.AddAsync(configuredEntityTask.Result.Result))
                .Result,
            _ => throw new ArgumentException()
        };
    }

    public override async Task UpdateAsync(TEntity oldEntity, TEntity newEntity)
    {
        switch (Configuration)
        {
            case { OneToManyUpdatingType: OneToManyUpdatingType.ForeignPropertiesMustExist }:
                {
                    var properties = typeof(TEntity).GetProperties();
                    foreach (var property in RelationShipResolver.ToToAnotherProperties(newEntity))
                    {
                        var relatedEntityId = await GetIdOfExisting(property.GetValue(newEntity) as DomainModelBase);
                        properties.First(p => p.Name == Configuration.ForeignPropertyNameFor(property.PropertyType))
                            .SetValue(newEntity, relatedEntityId);
                    }
                    await base.UpdateAsync(oldEntity, newEntity);
                    break;
                }
            default: throw new ArgumentOutOfRangeException();
        }
    }
}