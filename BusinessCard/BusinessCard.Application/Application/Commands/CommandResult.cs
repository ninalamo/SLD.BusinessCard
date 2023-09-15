using System.Text.Json;

namespace BusinessCard.Application.Application.Commands;

public record CommandResult
{
    private CommandResult(bool isSuccess, string errorMessage, object[] data)
    {
        ErrorMessage = errorMessage;
        IsSuccess = isSuccess;
        Data = data;
    }
    
    

    public string ErrorMessage { get; }
    public bool IsSuccess { get; }
    public object[] Data { get; }

    private static CommandResult Create(bool isSuccess, string errorMessage, object[] data = null)
    {
        return new CommandResult(isSuccess, errorMessage, data);
    }

    public static CommandResult Success(object[] data = null)
    {
        return Create(true, string.Empty, data ?? Array.Empty<object>());
    }
    
    public static CommandResult Success(object data )
    {
        return Create(true, string.Empty, data == null ? Array.Empty<object>() : new []{data});
    }
    
    public static CommandResult Success()
    {
        return Create(true, string.Empty,  Array.Empty<object>());
    }

    public static CommandResult Failed(string errorMessage)
    {
        return Create(false, errorMessage, null);
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}