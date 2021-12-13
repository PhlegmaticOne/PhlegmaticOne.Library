using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.CRUDs;
using PhlegmaticOne.Library.Domain.Models;
using PhlegmaticOne.Library.Domain.Services;
using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Extensions;

namespace PhlegmaticOne.Library.Database.DB;

public class AdoDataService : IDataService, IAsyncDisposable
{
    private readonly SqlDbCrudsFactory _sqlDbCrudsFactory;
    private SqlConnection _connection;
    private AdoDataService(SqlDbCrudsFactory sqlDbCrudsFactory) => _sqlDbCrudsFactory = sqlDbCrudsFactory;

    internal static async Task<AdoDataService> CreateInstanceAsync(IConnectionStringGetter connectionStringGetter,
                                                                   SqlDbCrudsFactory sqlDbCrudsFactory)
    {
        var instance = new AdoDataService(sqlDbCrudsFactory);
        instance._connection = new SqlConnection(connectionStringGetter.GetConnectionString());
        await instance._connection.OpenAsync();
        return instance;
    }
    public async Task<int?> AddAsync<TEntity>(TEntity entity) where TEntity : DomainModelBase =>
        await _sqlDbCrudsFactory.SqlCrudFor<TEntity>(_connection).AddAsync(entity);
    public async Task<TEntity> GetLazyAsync<TEntity>(int id) where TEntity : DomainModelBase =>
        await _sqlDbCrudsFactory.SqlCrudFor<TEntity>(_connection).GetLazy<TEntity>(id);

    public Task<DeleteCommandResult<TEntity>> DeleteAsync<TEntity>(int id) where TEntity : DomainModelBase
    {
        throw new NotImplementedException();
    }
    public Task<UpdateCommandResult<TEntity>> UpdateAsync<TEntity>(int id, TEntity newEntity) where TEntity : DomainModelBase
    {
        throw new NotImplementedException();
    }
    public async Task<TEntity> GetFullAsync<TEntity>(int id) where TEntity : DomainModelBase
    {
        return await _sqlDbCrudsFactory.SqlCrudFor<TEntity>(_connection).GetFull<TEntity>(id);
    }
    public async Task<int> GetIdOfExisting<TEntity>(TEntity entity) where TEntity : DomainModelBase
    {
        //var expression = _sqlCommandBuilder.SelectIdExpression(entity);
        //var reader = await Parametrize(new SqlCommand(expression, _connection), entity).ExecuteReaderAsync();
        //int id = default;
        //while (reader.Read()) id = reader.GetInt32(0);
        return await Task.Run(() => 3);
    }

    public async Task<int> EnsureDeletedAsync()
    {
        var list = new List<string>();
        await using var readCommand = new SqlCommand(SqlCommandsPatterns.AllDeletingStatements(), _connection);
        await using (var rdr = await readCommand.ExecuteReaderAsync())
        {
            while (await rdr.ReadAsync()) list.Add(rdr.GetString(0));
        }
        var totalDeleted = 0;
        foreach (var commandText in list)
        {
            await using var command = new SqlCommand(commandText, _connection);
            totalDeleted += await command.ExecuteNonQueryAsync();
        }
        return totalDeleted;
    }
    public async Task<IEnumerable<object>> ExecuteCommand(string commandText)
    {
        var result = new List<object>();
        await using var command = new SqlCommand(commandText, _connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                result.Add(reader.GetValue(i));
            }
        }
        return result;
    }
    public ValueTask DisposeAsync() => _connection.DisposeAsync();
}