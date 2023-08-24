using BusinessCard.API.Application.Queries.GetClients;
using BusinessCard.API.Application.Queries.GetMembers;
using ClientService;

namespace BusinessCard.API.Application.Common.Interfaces;
public interface IClientQueries
{
    Task<(int,IEnumerable<ClientsResult>)> GetClientsWithPagination(int pageSize, int pageNumber, string? name);
    Task<ClientsResult> GetClientById(Guid id);
    Task<(int,IEnumerable<MembersResult>)> GetMembersWithPagination(int pageSize, int pageNumber, Guid clientId);
}