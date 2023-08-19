using System.Data;
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

        private IDbContextTransaction? _currentTransaction; //use in rollback-enabled transactions

        #region Constructor

        public LokiContext(DbContextOptions<LokiContext> options, IMediator mediator, ICurrentUser? currentUser) :
            base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _currentUser = currentUser;
        }

        #endregion

        public virtual DbSet<Client> Clients { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Card> Cards { get; set; }




        public bool HasActiveTransaction => _currentTransaction != null;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //this.DbAudit(_currentUser);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LokiContext).Assembly);
        }

        public IDbContextTransaction? GetCurrentTransaction()
        {
            return _currentTransaction;
        }

        public async Task<IDbContextTransaction?> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return default;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
