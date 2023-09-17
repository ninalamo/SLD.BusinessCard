namespace BusinessCard.Application.Application.Common.SQLScripts;

internal sealed class SubscriptionSQL
{
    public const string SelectSubscriptionsByClientId =
        @"SELECT 
            [Id]
          ,[StartDate]
          ,[EndDate]
          ,[ActualEndDate]
          ,[State]
          ,[Reason]
          ,[PaymentScheduleReminderInterval]
          ,[PaymentScheduleInterval]
          ,[ClientId]
          ,[BillingPlanId]
          ,[CreatedBy]
          ,[CreatedOn]
          ,[ModifiedBy]
          ,[ModifiedOn]
          ,[IsActive]
          ,[CardExpiryInMonths]
          ,[Description]
          ,[Level]
      FROM [kardb].[dbo].[subscription] 
      WHERE [ClientId] = @ClientId ";
}