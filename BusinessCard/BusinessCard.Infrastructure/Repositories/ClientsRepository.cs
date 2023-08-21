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
    
    public async Task<Client> CreateAsync(string name, bool isDiscreet, Guid tierId)
    {
        var entity = await _context.Clients.AddAsync(new Client(name, isDiscreet, tierId));
        var state = _context.Entry(entity).State;
        return entity.Entity;
    }

    public Client Update(Client client)
    {
        try
        {
            var entity = _context.Clients.Update(client);
            var state = _context.Entry(client).State;
            return entity.Entity;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Client> GetEntityByIdAsync(Guid id) =>  _context.Clients.FirstOrDefault(c => c.Id == id);

    public async Task<Client> GetWithPropertiesByIdAsync(Guid id)
    {
        var entity =  _context.Clients
            .Include(c => c.Persons)
            .ThenInclude(p => p.Card)
            .FirstOrDefault(c => c.Id == id);
        return entity;
    }

}