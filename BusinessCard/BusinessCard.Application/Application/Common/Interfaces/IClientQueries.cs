using BusinessCard.API.Application.Queries.GetClients;
using BusinessCard.API.Application.Queries.GetMembers;

namespace BusinessCard.Application.Application.Common.Interfaces;
public interface IClientQueries
{
    Task<(int,IEnumerable<ClientsResult>)> GetClientsWithPagination(int pageSize, int pageNumber, string? name);
    Task<ClientsResult> GetClientById(Guid id);
    Task<MembersResult> GetClientByUid(string uid);
    Task<(int,IEnumerable<MembersResult>)> GetMembersWithPagination(int pageSize, int pageNumber, Guid clientId);
}