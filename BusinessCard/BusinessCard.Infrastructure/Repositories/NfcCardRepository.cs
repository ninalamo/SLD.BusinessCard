using BusinessCard.Domain.AggregatesModel.NFCAggregate;
using BusinessCard.Domain.Exceptions;
using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Infrastructure.Repositories
{
    public class NfcCardRepository : ICardRepository
    {
        private readonly MyDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public NfcCardRepository(MyDbContext context)
        {
            _context = context ?? throw BusinessCardDomainException.CreateArgumentNullException(nameof(context));
        }

        public NfcCard Create(NfcCard card)
        {
            throw new NotImplementedException();
        }

        public Task CreateBatch(IEnumerable<NfcCard> cards)
        {
            throw new NotImplementedException();
        }
    }
}
