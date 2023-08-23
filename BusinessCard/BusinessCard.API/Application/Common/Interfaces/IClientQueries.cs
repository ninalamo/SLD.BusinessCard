using BusinessCard.API.Application.Queries.GetClients;

namespace BusinessCard.API.Application.Common.Interfaces;
public interface IClientQueries
{
    Task<(int,IEnumerable<ClientsResult>)> GetClientsWithPagination(int pageSize, int pageNumber, string? name);
    Task<ClientsResult> GetClientbyId(Guid id);
}