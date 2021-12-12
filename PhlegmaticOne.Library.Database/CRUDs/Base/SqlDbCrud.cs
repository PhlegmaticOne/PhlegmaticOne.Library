using System.Data;
using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.CRUDs.Base;

public abstract class SqlDbCrud : DbCrudBase
{
    protected readonly SqlConnection Connection;
    protected readonly ISqlCommandExpressionProvider ExpressionProvider;
    protected readonly DataContextConfigurationBase<AdoDataService> Configuration;
    protected SqlDbCrud(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                        DataContextConfigurationBase<AdoDataService> configuration )
    {
        Connection = connection;
        ExpressionProvider = expressionProvider;
        Configuration = configuration;
    }
    public override async Task<int?> AddAsync<TEntity>(TEntity entity)
    {
        return await Task.Factory.StartNew(() => AddConfiguredEntity(entity))
                                 .ContinueWith(_ => GetLastIdOf<TEntity>()).Result;
    }

    public override void Update<TEntity>(int id, TEntity entity)
    {
        throw new NotImplementedException();
    }

    public override void Delete<TEntity>(int id)
    {
        throw new NotImplementedException();
    }

    public override void GetLazy<TEntity>()
    {
        throw new NotImplementedException();
    }

    public override void GetFull<TEntity>()
    {
        throw new NotImplementedException();
    }

    protected void AddConfiguredEntity<TEntity>(TEntity entity) where TEntity : DomainModelBase
    {
        var properties = typeof(TEntity).GetProperties()
            .WithoutAppearance<DomainModelBase>(p => p.Name != "Id")
            .ToDictionary(key => key.Name, value => value.GetValue(entity));
        var adapter = new SqlDataAdapter(SqlCommandsPatterns.EmptySelectFor<TEntity>(), Connection);
        var dataSet = new DataSet(); 
        adapter.Fill(dataSet);
        var table = dataSet.Tables.First();
        table.Rows.Add(table.NewRow().ParametrizeWith(properties));
        new SqlCommandBuilder(adapter);
        adapter.Update(dataSet);
    }
    protected async Task<int?> GetLastIdOf<TEntity>() where TEntity: DomainModelBase =>
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