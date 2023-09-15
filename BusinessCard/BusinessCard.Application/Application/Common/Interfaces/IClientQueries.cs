using BusinessCard.Application.Application.Queries.GetClients;
using BusinessCard.Application.Application.Queries.GetMembers;

namespace BusinessCard.Application.Application.Common.Interfaces;
public interface IClientQueries
{
    Task<bool> IsCardExists(string uid);
    Task<(int,IEnumerable<ClientsResult>)> GetClientsWithPagination(int pageSize, int pageNumber, string? name);
    Task<ClientsResult> GetClientById(Guid id);
    Task<MembersResult> GetClientByUid(string uid);
    Task<(int,IEnumerable<MembersResult>)> GetMembersWithPagination(int pageSize, int pageNumber, Guid clientId);
}