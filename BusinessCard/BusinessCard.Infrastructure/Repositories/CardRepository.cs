using BusinessCard.Domain.AggregatesModel.NFCAggregate;
using BusinessCard.Domain.Exceptions;
using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Infrastructure.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly CardDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public CardRepository(CardDbContext context)
        {
            _context = context ?? throw BusinessCardDomainException.CreateArgumentNullException(nameof(context));
        }

        public NfcCard Create(NfcCard card)
        {
            throw new NotImplementedException();
        }

        public Task CreateBatchAsync(IEnumerable<NfcCard> cards)
        {
            throw new NotImplementedException();
        }

        public void ToggleActive(string key, bool isActive = false)
        {
            throw new NotImplementedException();
        }

        public Task ToggleBatchActive(Guid companyID, bool isActive = false)
        {
            throw new NotImplementedException();
        }
    }
}
