namespace BusinessCard.API.Exceptions;

public class BusinessCardApiException : Exception
{
    private const string MESSAGE = "BUSINESS_CARD_API_EXCEPTION was caught.";
    public BusinessCardApiException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static BusinessCardApiException Create(Exception? innerException) => new(MESSAGE, innerException);
    public static BusinessCardApiException CreateArgumentNullException(string nameOfParam) => new(MESSAGE, new ArgumentNullException($"{nameOfParam} is null."));

}