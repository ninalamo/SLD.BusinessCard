namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public interface IClientsRepository : IRepository<Client>
{
    Client Create(string name, bool isDiscreet, Guid tierId);
    Client Update(Client client);
    Task<Client> GetEntityByIdAsync(Guid id);
    Task<Client> GetWithPropertiesByIdAsync(Guid id);

}