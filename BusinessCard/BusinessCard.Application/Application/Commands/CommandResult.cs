using System.Text.Json;

namespace BusinessCard.Application.Application.Commands;

public record CommandResult
{
    private CommandResult(bool isSuccess, string errorMessage, Guid? id)
    {
        ErrorMessage = errorMessage;
        IsSuccess = isSuccess;
        Id = id;
    }

    public string ErrorMessage { get; }
    public bool IsSuccess { get; }
    public Guid? Id { get; }

    private static CommandResult Create(bool isSuccess, string errorMessage, Guid? id)
    {
        return new CommandResult(isSuccess, errorMessage, id);
    }

    public static CommandResult Success(Guid? id)
    {
        return Create(true, string.Empty, id);
    }

    public static CommandResult Failed(Guid? id, string errorMessage)
    {
        return Create(false, errorMessage, id);
    }

    public static CommandResult Failed(Guid? id, Type type)
    {
        return Failed(id, $"{type} with {id} not found.");
    }

    public static CommandResult EntityNotFoundResult(Entity entity)
    {
        return Failed(entity.Id, entity.GetType());
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}