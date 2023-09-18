namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public interface IClientsRepository : IRepository<Client>
{
    Task<Client> CreateAsync(string  name, string tierId);
    Client Update(Client client);
    Task<Client> GetEntityByIdAsync(Guid id);
    Task<Client> GetWithPropertiesByIdAsync(Guid id);
}