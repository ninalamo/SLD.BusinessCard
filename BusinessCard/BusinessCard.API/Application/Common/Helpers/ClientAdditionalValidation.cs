using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.API.Application.Common.Interfaces.Helpers;

public static class AdditionalValidationExtensions
{
   public static void AdditionalValidation(this Client client, string phoneNumber, string email, Guid? memberId = null)
   {
      var validationFailures = new List<ValidationFailure>();
      if (client == null) throw new KeyNotFoundException("Client not found.");

      if (memberId.HasValue && memberId.Value != Guid.Empty)
      {
         var person = client.Persons.FirstOrDefault(p => p.Id == memberId.Value);
       
         if (person == null) throw new KeyNotFoundException("Member not found.");

         if (client.Persons.Any(i => i.PhoneNumber == phoneNumber && i.Id != memberId.Value))
            validationFailures.Add(new ValidationFailure("PhoneNumber",
               $"PhoneNumber: ({phoneNumber}) Already exists."));

         if (client.Persons.Any(i => i.Email.ToLower() == email.ToLower() && i.Id != memberId.Value))
            validationFailures.Add(new ValidationFailure("Email", $"Email: ({email}) Already exists."));

      }

      if (validationFailures.Any())
         throw new ValidationException("Business validation error.", validationFailures);
   } 
}

