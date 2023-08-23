namespace BusinessCard.API.Application.Common.SQLScripts;

internal sealed class SqlScript
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
  			LEFT JOIN kardb.kardibee.people P ON P.ClientId = C.Id 
  			LEFT JOIN kardb.kardibee.membertier M ON M.Id = C.MemberTierId ";
    
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
  			LEFT JOIN kardb.kardibee.people P ON P.ClientId = C.Id 
  			LEFT JOIN kardb.kardibee.membertier M ON M.Id = C.MemberTierId 
  		WHERE C.[Id] = @Id ";

    public const string ClientCount = @"SELECT COUNT(Id) FROM kardb.kardibee.client";
}