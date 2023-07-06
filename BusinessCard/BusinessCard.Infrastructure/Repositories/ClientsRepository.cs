using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;
using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Infrastructure.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly LokiContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public ClientsRepository(LokiContext context)
    {
        _context = context ?? throw BusinessCardDomainException.CreateArgumentNullException(nameof(context));
    }
    
    public Client Create(string name, bool isDiscreet, Tier subsription)
    {
        return _context.Clients.Add(new Client(name, isDiscreet, subsription)).Entity;
    }

    public Client Update(Client client)
    {
        return _context.Clients.Update(client).Entity;
    }
}