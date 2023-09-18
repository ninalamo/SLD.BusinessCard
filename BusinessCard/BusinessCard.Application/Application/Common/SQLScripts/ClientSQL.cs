namespace BusinessCard.Application.Application.Common.SQLScripts;

internal static class ClientSQL
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

    public const string SelectMembers = 
	@"SELECT 
	    ISNULL([Uid],'') [CardKey]
	    ,P.[Id] [MemberId]
	    ,[SubscriptionId]
	    ,[ClientId]
	    ,[FirstName]
	    ,[MiddleName]
	    ,[LastName]
	    ,[NameSuffix]
	    ,[Email]
	    ,[Address]
	    ,[PhoneNumber]
	    ,C.Name [Company]
	    ,[IdentityUserId]
	    ,[Facebook]
	    ,[LinkedIn]
	    ,[Instagram]
	    ,[Pinterest]
	    ,[Twitter]
	    ,[Occupation]
	    ,[Level] [CardLevel]
	FROM [kardb].[dbo].client C 
        JOIN [kardb].[dbo].[subscription] S ON S.ClientId = C.Id
        JOIN [kardb].[dbo].[people] P ON P.SubscriptionId = S.Id
        LEFT JOIN [kardb].[dbo].[card] K ON K.PersonId = P.Id
        LEFT JOIN [kardb].[dbo].[socialmedia] SC ON SC.PersonId = P.Id 
";

    public const string ClientCount = @"SELECT COUNT(Id) FROM kardb.dbo.client WHERE [IsActive] = 1 ";
    
    public const string MemberCount = 
	    @"SELECT 
    		COUNT(*)
		FROM [kardb].[dbo].[people] P
  			LEFT JOIN [kardb].[dbo].subscription S ON S.Id = P.SubscriptionId
    		LEFT JOIN [kardb].[dbo].[client] C ON C.Id = S.ClientId 
  		WHERE ClientId = @ClientId 
";

    public const string CheckIfCardKeyExists = @"SELECT COUNT([Key]) [Count] FROM [kardb].[kardibee].[card] WHERE [Key] = @key";
    
}