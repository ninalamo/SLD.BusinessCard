namespace BusinessCard.Application.Application.Common.SQLScripts;

public class CardSQL
{
    public const string SelectCardByUidAndClientId =
        @" SELECT 
            K.[Id]
            ,K.[Uid]
            ,P.[Id] [MemberId]
            ,S.[Id] [SubscriptionId]
            ,C.[Id] [ClientId]
        FROM [kardb].[dbo].[card] K
            LEFT JOIN [kardb].[dbo].[people] P ON P.Id = K.PersonId
            LEFT JOIN [kardb].[dbo].[subscription] S ON S.Id = P.SubscriptionId
            LEFT JOIN [kardb].[dbo].[client] C ON C.Id = S.ClientId
        WHERE K.[Uid] = @Uid AND C.[Id] = @ClientId ";

    public const string SelectCardIfExists =
        @" SELECT COUNT(*) [Exists]
        FROM [kardb].[dbo].[card] K
            LEFT JOIN [kardb].[dbo].[people] P ON P.Id = K.PersonId
            LEFT JOIN [kardb].[dbo].[subscription] S ON S.Id = P.SubscriptionId
            LEFT JOIN [kardb].[dbo].[client] C ON C.Id = S.ClientId
          WHERE K.[Uid] = @Uid AND C.[Id] = @ClientId ";
}