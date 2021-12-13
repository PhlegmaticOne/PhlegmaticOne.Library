using PhlegmaticOne.Library.Database.Configuration.Base;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Extensions;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace PhlegmaticOne.Library.Database.CRUDs.Base;

public abstract class SqlDbCrud : DbCrudBase
{
    protected readonly SqlConnection Connection;
    protected readonly ISqlCommandExpressionProvider ExpressionProvider;
    protected readonly DataContextConfigurationBase<AdoDataService> Configuration;
    protected SqlDbCrud(SqlConnection connection, ISqlCommandExpressionProvider expressionProvider,
                        DataContextConfigurationBase<AdoDataService> configuration)
    {
        Connection = connection;
        ExpressionProvider = expressionProvider;
        Configuration = configuration;
    }
    public override async Task<int?> AddAsync<TEntity>(TEntity entity) =>
        await Task.Factory.StartNew(() => AddConfiguredEntity(entity)).ContinueWith(_ => GetLastIdOf<TEntity>()).Result;
    public override async Task<TEntity> GetLazy<TEntity>(int id)
    {
        var entity = Activator.CreateInstance<TEntity>();
        var properties = typeof(TEntity).GetProperties();
        var expression = ExpressionProvider.SelectLazyByIdExpression<TEntity>(id);
        await using var command = new SqlCommand(expression, Connection);
        await using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            properties.First(p => p.Name == reader.GetName(i)).SetValue(entity, reader.GetValue(i));
        }
        return entity;
    }
    public override async Task<TEntity> GetFull<TEntity>(int id) => await GetLazy<TEntity>(id);
    public override void Update<TEntity>(int id, TEntity entity)
    {
        throw new NotImplementedException();
    }

    public override void Delete<TEntity>(int id)
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

    protected async Task<DomainModelBase> ToGeneric(Type entityType, int id)
    {
        return entityType.Name switch
        {
            "Gender" => await GetLazy<Gender>(id),
            "Genre" => await GetLazy<Genre>(id),
            "Abonent" => await GetLazy<Abonent>(id),
            "Author" => await GetLazy<Author>(id),
            "Book" => await GetLazy<Book>(id),
            "Lending" => await GetLazy<Lending>(id),
            "State" => await GetLazy<State>(id),
            _ => throw new ArgumentException()
        };
    }

    protected async Task<int?> GetLastIdOf<TEntity>() where TEntity : DomainModelBase =>
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

//internal class SelectingAlgorithms
//{
//    public async Task<TEntity> GetLazy<TEntity>(int id)
//    {
//        var entity = Activator.CreateInstance<TEntity>();
//        var properties = typeof(TEntity).GetProperties();
//        var expression = ExpressionProvider.SelectLazyByIdExpression<TEntity>(id);
//        await using var command = new SqlCommand(expression, Connection);
//        await using var reader = await command.ExecuteReaderAsync();
//        await reader.ReadAsync();
//        for (int i = 0; i < reader.FieldCount; i++)
//        {
//            properties.First(p => p.Name == reader.GetName(i)).SetValue(entity, reader.GetValue(i));
//        }
//        return entity;
//    }
//    public async Task<TEntity> GetSingleFull<TEntity>(int id) => await GetLazy<TEntity>(id);
//    public async Task<TEntity> GetToAnotherFull<TEntity>(int id)
//    { 
//        var entity = await GetLazy<TEntity>(id);
//    }
//    public async Task<TEntity> GetToManyFull<TEntity>(int id)
//    {
//        var entity = await GetLazy<TEntity>(id);
//    }
//    public async Task<TEntity> GetCompositeFull<TEntity>(int id)
//    {
//        var entity = await GetLazy<TEntity>(id);
//    }
//}