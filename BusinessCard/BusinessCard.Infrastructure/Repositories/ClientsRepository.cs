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
    
    public async Task<Client> CreateAsync(string name, bool isDiscreet, string industry)
    {
        var entity = await _context.Clients.AddAsync(new Client(name, isDiscreet, industry));
        return entity.Entity;
    }

    public Client Update(Client client)
    {
        var entity = _context.Clients.Update(client);
        return entity.Entity;
    }

    public async Task<Client> GetEntityByIdAsync(Guid id) =>  await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Client> GetWithPropertiesByIdAsync(Guid id)
    {
        var entity =  await _context.Clients
            .Include(c => c.Persons).ThenInclude(p => p.Cards)
            .Include(c => c.Persons)//.ThenInclude(p => p.MemberTier)
           // .Include(c => c.MembershipTier)
            .FirstOrDefaultAsync(c => c.Id == id);
        return entity;
    }

}