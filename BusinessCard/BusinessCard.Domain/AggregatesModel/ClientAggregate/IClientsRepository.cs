namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public interface IClientsRepository : IRepository<Client>
{
    Client Create(string name, bool isDiscreet, Tier subscription);
    Client Update(Client client);
        
}