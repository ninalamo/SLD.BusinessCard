namespace BusinessCard.API.Application.Common.SQLScripts;

internal static class SqlScript
{
    public const string SelectClients = @"SELECT 
	  		C.[Id] [ClientId] 
      		,C.[CompanyName] 
      		,C.[IsDiscreet] 
      		,C.[CreatedBy] 
      		,C.[CreatedOn] 
      		,C.[ModifiedBy] 
      		,C.[ModifiedOn] 
      		,C.[IsActive] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 1)  [Cardholders] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 0)  [NonCardholders] 
  		FROM [kardb].[kardibee].[client] C  ";
    
    public const string SelectClientById =  @"SELECT TOP 1
	  		C.[Id] [ClientId] 
      		,C.[CompanyName] 
      		,C.[IsDiscreet] 
      		,C.[CreatedBy] 
      		,C.[CreatedOn] 
      		,C.[ModifiedBy] 
      		,C.[ModifiedOn] 
      		,C.[IsActive] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 1)  [Cardholders] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 0)  [NonCardholders] 
  		FROM [kardb].[kardibee].[client] C 
  		WHERE C.[Id] = @Id ";

    public const string SelectMembers = @"SELECT
    		P.[Id] 
	      ,P.[IsSubscriptionOverride]
	      ,P.[FirstName]
	      ,P.[LastName]
	      ,P.[MiddleName]
	      ,P.[NameSuffix]
	      ,P.[PhoneNumber]
	      ,P.[Email]
	      ,P.[Address]
	      ,P.[SocialMedia]
	      ,P.[Occupation]
	      ,P.[CardId]
		  ,C.[Key] [CardKey]
	      ,P.[ClientId]
		  ,CL.[CompanyName] [Company]
	      ,P.[CreatedBy]
	      ,P.[CreatedOn]
	      ,P.[ModifiedBy]
	      ,P.[ModifiedOn]
	      ,P.[IsActive]
    	  ,P.[IdentityUserId]
	FROM [kardb].[kardibee].[people] P
	LEFT JOIN kardb.kardibee.client CL ON CL.Id = P.ClientId
	LEFT JOIN kardb.kardibee.card C ON C.Id = P.CardId ";

    public const string ClientCount = @"SELECT COUNT(Id) FROM kardb.kardibee.client";
    
    public const string MemberCount = @"SELECT COUNT(Id) FROM kardb.kardibee.people WHERE ClientId = @ClientId";

    public const string CheckIfCardKeyExists = @"SELECT COUNT([Key]) [Count] FROM [kardb].[kardibee].[card] WHERE [Key] = @key";
    
}