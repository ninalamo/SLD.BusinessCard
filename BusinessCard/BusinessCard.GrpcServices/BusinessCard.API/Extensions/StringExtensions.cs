using FluentValidation.Results;

namespace BusinessCard.API.Extensions;

public static class StringExtensions
{
    public static DateTimeOffset ToDateTimeOffset(this string str)
    {
        if (!DateTimeOffset.TryParse(str, out var parsedDate))
        {
            throw new ValidationException("Validation Error", new[] { new ValidationFailure( "Date", "Date is invalid.") });
        }

        return parsedDate;
    }
}