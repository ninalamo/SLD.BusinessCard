namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public interface IClientsRepository : IRepository<Client>
{
    Task<Client> CreateAsync(string name, bool isDiscreet, int tierId);
    Client Update(Client client);
    Task<Client> GetEntityByIdAsync(Guid id);
    Task<Client> GetWithPropertiesByIdAsync(Guid id);
}