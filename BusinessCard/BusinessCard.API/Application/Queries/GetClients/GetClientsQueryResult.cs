using System.Text.Json.Serialization;

namespace BusinessCard.API.Application.Queries.GetClients;

public record GetClientsQueryResult
{
    public GetClientsQueryResult(int pageSize, int pageNumber, int totalCount, IEnumerable<ClientsResult> clients)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalCount = totalCount;
        Clients = clients;
    }
    [JsonPropertyName("page_size")]
    public int PageSize { get; private set; }
    [JsonPropertyName("page_number")]
    public int PageNumber { get; private set; }
    [JsonPropertyName("total_count")]
    public int TotalCount { get; private set; }
    [JsonPropertyName("clients")]
    public IEnumerable<ClientsResult> Clients { get; private set; }
}

public record ClientsResult
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; init; }
    [JsonPropertyName("company_name")]
    public string CompanyName {get;  init; }
    [JsonPropertyName("is_discreet")]
    public bool IsDiscreet { get; init; }
    [JsonPropertyName("subscription_level")]
    public int SubscriptionLevel { get;init; }
    [JsonPropertyName("subscription")]
    public string Subscription { get; init; }
    [JsonPropertyName("card_holders")]
    public int CardHolders { get; init; }
    [JsonPropertyName("non_cardholders")]
    public int NonCardHolders { get; init; }
    [JsonPropertyName("created_by")]
    public string? CreatedBy { get; init; }
    [JsonPropertyName("modified_by")]
    public string? ModifiedBy { get; init; }
    [JsonPropertyName("card_id")]
    public string? CardId { get; init; }
    [JsonPropertyName("card_key")]
    public string? CardKey { get; init; }
    [JsonPropertyName("created_on")]
    public DateTimeOffset? CreatedOn { get; init; }
    [JsonPropertyName("modified_on")]
    public DateTimeOffset? ModifiedOn { get; init; }
    [JsonPropertyName("is_active")]
    public bool IsActive { get; init; }
}
