using System.Text.Json.Serialization;

namespace BusinessCard.Application.Application.Common.Models;

public class PaginationQueryResult<T>
{
    public PaginationQueryResult(int pageSize, int pageNumber, int totalCount, IEnumerable<T> data)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalCount = totalCount;
        TotalPages = totalCount % pageSize > 0 ? totalCount / pageSize + 1 : totalCount / pageSize;
        Data = data;
    }

    [JsonPropertyName("page_size")] public int PageSize { get; }

    [JsonPropertyName("page_number")] public int PageNumber { get; }

    [JsonPropertyName("total")] public int TotalCount { get; }

    [JsonPropertyName("total_pages")] public int TotalPages { get; }

    [JsonPropertyName("data")] public IEnumerable<T> Data { get; }
}