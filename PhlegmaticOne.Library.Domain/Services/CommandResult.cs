using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Domain.Services;

public abstract class CommandResult<TEntity> where TEntity : DomainModelBase
{
    public abstract CommandType CommandType { get; }
    public string Message { get; }
    public TEntity Entity { get; }
    public bool IsSuccessful { get; }
    protected CommandResult(string message, TEntity entity, bool isSuccessful)
    {
        Message = message;
        Entity = entity;
        IsSuccessful = isSuccessful;
    }
}

public class AddCommandResult<TEntity> : CommandResult<TEntity> where TEntity : DomainModelBase
{
    public override CommandType CommandType => CommandType.Add;
    public AddCommandResult(string message, TEntity entity, bool isSuccessful) : base(message, entity, isSuccessful) { }
}

public class DeleteCommandResult<TEntity> : CommandResult<TEntity> where TEntity : DomainModelBase
{
    public override CommandType CommandType => CommandType.Delete;
    public DeleteCommandResult(string message, TEntity entity, bool isSuccessful) : base(message, entity, isSuccessful) { }
}

public class UpdateCommandResult<TEntity> : CommandResult<TEntity> where TEntity : DomainModelBase
{
    public override CommandType CommandType => CommandType.Update;
    public UpdateCommandResult(string message, TEntity entity, bool isSuccessful) : base(message, entity, isSuccessful) { }
}

public class GetCommandResult<TEntity> : CommandResult<TEntity> where TEntity : DomainModelBase
{
    public override CommandType CommandType => CommandType.Get;
    public GetCommandResult(string message, TEntity entity, bool isSuccessful) : base(message, entity, isSuccessful) { }
}

public enum CommandType
{
    Add,
    Delete,
    Get,
    Update
}