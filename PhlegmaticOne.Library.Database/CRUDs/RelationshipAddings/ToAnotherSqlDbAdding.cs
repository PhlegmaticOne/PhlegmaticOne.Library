using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings;

public class ToAnotherSqlDbAdding<T> : SqlDbAdding<T> where T : DomainModelBase
{
    public ToAnotherSqlDbAdding(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                                DataContextConfigurationBase<AdoDataService> configuration, IRelationShipResolver relationShipResolver) :
                                base(connection, expressionProvider, configuration, relationShipResolver)
    { }
    public override async Task<int?> AddAsync(T entity)
    {
        return Configuration switch
        {
            { OneToManyAddingType: OneToManyAddingType.ForeignPropertiesMustExist} =>
                await Task.Factory.StartNew(async () =>
                {
                    var properties = typeof(T).GetProperties();
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
}