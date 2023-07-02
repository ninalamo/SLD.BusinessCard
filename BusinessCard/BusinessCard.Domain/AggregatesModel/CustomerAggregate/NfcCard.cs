using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.CustomerAggregate
{
    public class NfcCard : ValueObject
    {
        public string Key { get; }
        public Guid CompanyId { get; } = default;

        public NfcCard(string key, Guid companyId)
        {
            Key = key;
            CompanyId = companyId;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Key;
            yield return CompanyId;
        }
    }
}
