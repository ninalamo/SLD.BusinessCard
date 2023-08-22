using BusinessCard.API.Application.Queries.GetClients;

namespace BusinessCard.API.Application.Queries;
public interface IClientQueries
{
    Task<(int,IEnumerable<ClientsResult>)> GetClientsWithPagination(int pageSize, int pageNumber, string? name);
}