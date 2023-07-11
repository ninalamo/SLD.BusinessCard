using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;
using BusinessCard.Domain.Seedwork;
using Microsoft.EntityFrameworkCore;

namespace BusinessCard.Infrastructure.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly LokiContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public ClientsRepository(LokiContext context)
    {
        _context = context ?? throw BusinessCardDomainException.CreateArgumentNullException(nameof(context));
    }
    
    public Client Create(string name, bool isDiscreet, Guid tierId) => _context.Clients.Add(new Client(name, isDiscreet, tierId)).Entity;

    public Client Update(Client client) => _context.Clients.Update(client).Entity;

    public async Task<Client> GetEntityByIdAsync(Guid id) => await _context.Clients.Include(c => c.Persons).ThenInclude(p => p.Card).SingleOrDefaultAsync(c => c.Id == id);

    public async Task<Client> GetWithPropertiesByIdAsync(Guid id) =>  await _context.Clients.SingleOrDefaultAsync(c => c.Id == id);
}