using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Seedwork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BusinessCard.Infrastructure
{
    public class CardDbContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "card_db";
        private readonly ICurrentUser _currentUser; //use in audit trail
        private readonly IMediator _mediator; //use in domain events

        private IDbContextTransaction _currentTransaction; //use in rollback-enabled transactions

        #region Constructor
        public CardDbContext(DbContextOptions<CardDbContext> options, IMediator mediator, ICurrentUser currentUser = default) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _currentUser = currentUser;
        }
        #endregion

        public DbSet<Client> Companies { get; set; }
        public DbSet<Person> Employees { get; set; }
        public DbSet<Card> NfcCards { get; set; }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
