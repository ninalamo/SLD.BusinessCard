namespace BusinessCard.Application.Application.Common.SQLScripts;

internal static class SqlScript
{
    public const string SelectClients = 
		@"SELECT
			C.[Id]
    		,C.[Name]
    		,C.[Industry]   
  			,C.[IsActive]
			,(SELECT COUNT(*) FROM [kardb].[dbo].[subscription] S WHERE S.ClientId = C.[Id]) [Subscriptions]
  		FROM 
  			[kardb].[dbo].[client] C
  			LEFT JOIN [kardb].[dbo].[subscription] S ON S.[ClientId] = C.[Id]
  		WHERE
  			C.[IsActive] = 1 ";
    
    public const string SelectClientById = 
	    @"SELECT
			C.[Id]
    		,C.[Name]
    		,C.[Industry]   
  			,C.[IsActive]
			,(SELECT COUNT(*) FROM [kardb].[dbo].[subscription] S WHERE S.ClientId = C.[Id]) [Subscriptions]
  		FROM 
  			[kardb].[dbo].[client] C
  		WHERE 
  			[IsActive] = 1 AND [Id] = @Id ";

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
	FROM [kardb].[dbo].[people] P
	LEFT JOIN kardb.dbo.client CL ON CL.Id = P.ClientId
	LEFT JOIN kardb.dbo.card C ON C.Id = P.CardId ";

    public const string ClientCount = @"SELECT COUNT(Id) FROM kardb.dbo.client WHERE [IsActive] = 1 ";
    
    public const string MemberCount = @"SELECT COUNT(Id) FROM kardb.dbo.people WHERE ClientId = @ClientId";

    public const string CheckIfCardKeyExists = @"SELECT COUNT([Key]) [Count] FROM [kardb].[kardibee].[card] WHERE [Key] = @key";
    
}