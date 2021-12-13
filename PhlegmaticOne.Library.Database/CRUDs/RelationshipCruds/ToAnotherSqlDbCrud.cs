using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipCruds;

public class ToAnotherSqlDbCrud : SqlDbCrud
{
    public ToAnotherSqlDbCrud(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
        DataContextConfigurationBase<AdoDataService> configuration) :
        base(connection, expressionProvider, configuration)
    { }
    public override async Task<int?> AddAsync<TEntity>(TEntity entity)
    {
        switch (Configuration.OneToManyAddingType)
        {
            case OneToManyAddingType.ForeignPropertiesMustExist:
            default:
            {
                return await Task.Factory.StartNew(async () =>
                {
                    var properties = typeof(TEntity).GetProperties();
                    foreach (var property in properties.Where(p => p.PropertyType.IsAssignableTo(typeof(DomainModelBase))))
                    {
                        var relatedEntityId = await GetIdOfExisting(property.GetValue(entity) as DomainModelBase);
                        properties.First(p => p.Name == property.Name + "Id").SetValue(entity, relatedEntityId);
                    }
                    return entity;
                })
                .ContinueWith(async configuredEntityTask => await base.AddAsync(configuredEntityTask.Result.Result)).Result;
            }
        }
    }

    public override async Task<TEntity> GetFull<TEntity>(int id)
    {
        var lazyEntity = await GetLazy<TEntity>(id);
        var properties = typeof(TEntity).GetProperties();
        var relatedObjects = lazyEntity.PropertiesWithAppearance<DomainModelBase>();
        foreach (var property in relatedObjects)
        {
            var relatedId = properties.First(p => p.Name.Contains(property.Name + "Id")).GetValue(lazyEntity);
            DomainModelBase? relatedObject = default;
            if (relatedId is not null)
            {
                relatedObject = await ToGeneric(property.PropertyType, (int) relatedId);
            }
            property.SetValue(lazyEntity, relatedObject);
        }
        return lazyEntity;
    }

    private async Task<DomainModelBase> CommonGetFull(DomainModelBase entity)
    {
        var properties = entity.GetType().GetProperties()
            .Where(p => p.PropertyType.IsAssignableTo(typeof(DomainModelBase)));
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(entity) as DomainModelBase;
            var relatedEntityId = await GetIdOfExisting(propertyValue);
            var underlyingEntity = await GetFull<Book>(relatedEntityId.Value);
            property.SetValue(entity, underlyingEntity);
        }
        return entity;
    }
}