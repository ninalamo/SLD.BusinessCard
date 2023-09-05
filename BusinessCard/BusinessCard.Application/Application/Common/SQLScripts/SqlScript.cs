namespace BusinessCard.API.Application.Common.SQLScripts;

internal static class SqlScript
{
    public const string SelectClients = @"SELECT 
	  		C.[Id] [ClientId] 
      		,C.[CompanyName] 
      		,M.[Level] [SubscriptionLevel] 
      		,C.[IsDiscreet] 
      		,C.[CreatedBy] 
      		,C.[CreatedOn] 
      		,C.[ModifiedBy] 
      		,C.[ModifiedOn] 
      		,C.[IsActive] 
	  		,M.[Name] [Subscription] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 1)  [Cardholders] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 0)  [NonCardholders] 
  		FROM [kardb].[kardibee].[client] C 
  			LEFT JOIN kardb.kardibee.membertier M ON M.Id = C.[SubscriptionId] ";
    
    public const string SelectClientById =  @"SELECT TOP 1
	  		C.[Id] [ClientId] 
      		,C.[CompanyName] 
      		,M.[Level] [SubscriptionLevel] 
      		,C.[IsDiscreet] 
      		,C.[CreatedBy] 
      		,C.[CreatedOn] 
      		,C.[ModifiedBy] 
      		,C.[ModifiedOn] 
      		,C.[IsActive] 
	  		,M.[Name] [Subscription] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 1)  [Cardholders] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 0)  [NonCardholders] 
  		FROM [kardb].[kardibee].[client] C 
  			LEFT JOIN kardb.kardibee.membertier M ON M.Id = C.[SubscriptionId] 
  		WHERE C.[Id] = @Id ";

    public const string SelectMembers = @"SELECT
    		P.[Id] 
	      ,P.[IsSubsriptionOverride]
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
		  ,M.[Name] [Subscription]
		  ,M.[Level] [SubscriptionLevel]
	      ,P.[CreatedBy]
	      ,P.[CreatedOn]
	      ,P.[ModifiedBy]
	      ,P.[ModifiedOn]
	      ,P.[IsActive],
    	  ,P.[IdentityUserId]
	FROM [kardb].[kardibee].[people] P
	LEFT JOIN kardb.kardibee.card C ON C.Id = P.CardId
	LEFT JOIN kardb.kardibee.membertier M ON M.Id = P.[SubscriptionId] ";

    public const string ClientCount = @"SELECT COUNT(Id) FROM kardb.kardibee.client";
    
    public const string MemberCount = @"SELECT COUNT(Id) FROM kardb.kardibee.people WHERE ClientId = @ClientId";
}