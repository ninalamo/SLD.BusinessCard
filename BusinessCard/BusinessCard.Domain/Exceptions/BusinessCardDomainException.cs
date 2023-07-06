namespace BusinessCard.Domain.Exceptions
{
    public class BusinessCardDomainException : Exception
    {
        private const string MESSAGE = "BUSINESS_CARD_DOMAIN_EXCEPTION was caught.";
        public BusinessCardDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public static BusinessCardDomainException Create(Exception? innerException) => new(MESSAGE, innerException);
        public static BusinessCardDomainException CreateArgumentNullException(string nameOfParam) => new(MESSAGE, new ArgumentNullException($"{nameOfParam} is null."));


    }
}
