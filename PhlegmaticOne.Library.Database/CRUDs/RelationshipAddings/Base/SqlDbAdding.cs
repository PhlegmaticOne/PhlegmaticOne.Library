using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.CRUDs.RelationshipAddings.Base;

public abstract class SqlDbAdding<TEntity> where TEntity: DomainModelBase
{
    protected readonly SqlConnection Connection;
    protected readonly ISqlCommandExpressionProvider ExpressionProvider;
    protected readonly DataContextConfigurationBase<AdoDataService> Configuration;
    protected SqlDbAdding(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                        DataContextConfigurationBase<AdoDataService> configuration)
    {
        Connection = connection;
        ExpressionProvider = expressionProvider;
        Configuration = configuration;
    }
    public virtual async Task<int?> AddAsync(TEntity entity)=>
        await Task.Factory.StartNew(() => AddConfiguredEntity(entity))
                          .ContinueWith(_ => GetLastIdOf<TEntity>()).Result;

    protected void AddConfiguredEntity(TEntity entity)
    {
        IEnumerable<PropertyInfo> entityProperties = typeof(TEntity).GetProperties().WithoutAppearance<DomainModelBase>();

        if (entity.Id == 0)
        {
            entityProperties = entityProperties.Where(p => p.Name != Configuration.IdentificationPropertyName);
        }
        var properties = entityProperties.ToDictionary(key => key.Name, value => value.GetValue(entity));
        var adapter = new SqlDataAdapter(SqlCommandsPatterns.EmptySelectFor<TEntity>(), Connection);
        var dataSet = new DataSet();
        adapter.Fill(dataSet);
        var table = dataSet.Tables.First();
        table.Rows.Add(table.NewRow().ParametrizeWith(properties));
        new SqlCommandBuilder(adapter);
        adapter.Update(dataSet);
    }
    protected async Task<int?> GetLastIdOf<T>() where T : TEntity =>
        await ExecuteSimpleReadingCommand(SqlCommandsPatterns.GetLastIdFor<TEntity>());
    protected async Task<int?> GetIdOfExisting(DomainModelBase entity) =>
        await ExecuteSimpleReadingCommand(ExpressionProvider.SelectIdExpression(entity));
    private async Task<int?> ExecuteSimpleReadingCommand(string? commandText)
    {
        if (commandText is null) return null;
        await using var command = new SqlCommand(commandText, Connection);
        await using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return reader.GetInt32(0);
    }
}