namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{
    public sealed class Card : Entity
    {
        public string Uid { get; private set; }
        public DateTimeOffset? ActivatedDate { get; private set; }
        public DateTimeOffset? ExpireDate { get; private set; }
        public DateTimeOffset? RenewDate { get; private set; }
        

        public Card()
        {
            Uid = string.Empty;
            IsActive = false;
        }

        private void SaveUid(string key)
        {
            Uid = key;
        }

        public void Activate(string uid, int monthsBeforeExpire)
        {
            SaveUid(uid);
            
            ActivatedDate = DateTimeOffset.Now;
            ExpireDate = ActivatedDate.Value.AddMonths(monthsBeforeExpire);
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
            ExpireDate = DateTimeOffset.Now;
        }
        
        public void Renew(int monthsBeforeExpire, DateTimeOffset? renewDateOptional)
        {
            RenewDate = renewDateOptional ?? DateTimeOffset.Now;
            ExpireDate = RenewDate.Value.AddMonths(monthsBeforeExpire);
            IsActive = true;
        }

        public bool HasUid() => !string.IsNullOrEmpty(Uid);

    }
}
