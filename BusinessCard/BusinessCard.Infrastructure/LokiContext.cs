using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Seedwork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BusinessCard.Infrastructure
{
    public class LokiContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "kardibee";
        private readonly ICurrentUser? _currentUser; //use in audit trail
        private readonly IMediator _mediator; //use in domain events

        private IDbContextTransaction _currentTransaction; //use in rollback-enabled transactions

        #region Constructor
        public LokiContext(DbContextOptions<LokiContext> options, IMediator mediator, ICurrentUser? currentUser) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _currentUser = currentUser;
        }
        #endregion

        public DbSet<Client> Clients { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Card> Cards { get; set; }


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
