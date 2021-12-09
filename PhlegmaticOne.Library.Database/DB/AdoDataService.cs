using System.Data.SqlClient;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.SqlCommandBuilders.Base;
using PhlegmaticOne.Library.Domain.Models;
using PhlegmaticOne.Library.Domain.Services;

namespace PhlegmaticOne.Library.Database.DB;

public class AdoDataService : IDataService
{
    private readonly ISqlCommandExpressionBuilder _sqlCommandBuilder;
    private readonly string _connectionString;
    internal AdoDataService(IConnectionStringGetter connectionStringGetter,
                            ISqlCommandExpressionBuilder sqlCommandBuilder)
    {
        if (connectionStringGetter == null) throw new ArgumentNullException(nameof(connectionStringGetter));
        _sqlCommandBuilder = sqlCommandBuilder ?? throw new ArgumentNullException(nameof(sqlCommandBuilder));
        _connectionString = connectionStringGetter.GetConnectionString();
    }
    public async Task<bool> AddAsync(DomainModelBase entity)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        var expression = _sqlCommandBuilder.InsertExpression(entity);
        return await Parametrize(new SqlCommand(expression, connection), entity).ExecuteNonQueryAsync() == 1;
    }

    private SqlCommand Parametrize(SqlCommand command, DomainModelBase currentEntity)
    {
        foreach (var property in currentEntity.GetType().GetProperties().Where(p => p.Name != "Id"))
        {
            var propertyValue = property.GetValue(currentEntity);
            //if (property.PropertyType.IsAssignableTo(typeof(DomainModelBase)))
            //{
                
            //}

            //if (property.PropertyType.IsAssignableTo(typeof(ICollection<DomainModelBase>)))
            //{

            //}
            command.Parameters.Add(new SqlParameter("@" + property.Name, propertyValue));
        }
        return command;
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int id, DomainModelBase newEntity)
    {
        throw new NotImplementedException();
    }

    public Task<DomainModelBase> GetLazyAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DomainModelBase> GetFullAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetIdOfExisting(DomainModelBase entity)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        var expression = _sqlCommandBuilder.SelectIdExpression(entity);
        var reader = await Parametrize(new SqlCommand(expression, connection), entity).ExecuteReaderAsync();
        int id = default;
        while (reader.Read()) id = reader.GetInt32(0);
        return id;
    }
}