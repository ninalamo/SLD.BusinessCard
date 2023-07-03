﻿namespace BusinessCard.Domain.Exceptions
{
    public class BusinessCardDomainException : Exception
    {
        private const string MESSAGE = "BUSINESS_CARD_DOMAIN_EXCEPTION was caught.";
        private BusinessCardDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public static BusinessCardDomainException Create(Exception? innerException) => new(MESSAGE, innerException);


    }
}
