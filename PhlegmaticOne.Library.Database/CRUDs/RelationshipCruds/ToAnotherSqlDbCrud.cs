using System.Data;
using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipCruds;

public class ToAnotherSqlDbCrud : SqlDbCrud
{
    public ToAnotherSqlDbCrud(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
        DataContextConfigurationBase<AdoDataService> configuration) :
        base(connection, expressionProvider, configuration) { }
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
} 