using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.NFCAggregate
{
    public class NfcCard : Entity, IAggregateRoot
    {
        public string Key { get; }
        public Guid CompanyId { get; } = default;

        public NfcCard(string key, Guid companyId)
        {
            Key = key;
            CompanyId = companyId;
        }

        public static IEnumerable<NfcCard> GenerateEmptyCards(int count = 1)
        {
            List<NfcCard> cards = new();
            for (int i = 0; i < count; i++)
            {
                cards.Add(new(string.Empty, Guid.Empty));
            }

            return cards;
        }
    }

    public interface INfcAggregateRoot : IRepository<NfcCard> { 
        NfcCard Create(NfcCard card);
        Task CreateBatch(IEnumerable<NfcCard> cards);
    }
}
