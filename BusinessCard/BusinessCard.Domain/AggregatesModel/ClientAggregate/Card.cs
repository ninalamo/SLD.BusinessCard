using BusinessCard.Domain.Exceptions;
using FluentValidation;

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
            if(Uid != string.Empty)
            {
                throw  new ValidationException("Business validation error. NFC Key is immutable");
            }
            Uid = key;
        }

        public void Activate(string uid, int monthsBeforeExpire)
        {
            if (ActivatedDate.HasValue) throw new ValidationException("Already activated.");
            if (monthsBeforeExpire <= 0) throw new ValidationException("Should expire at least after 1 month.");

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
            if (!ActivatedDate.HasValue) throw new ValidationException("Cannot renew. Card inactive.");
            if (monthsBeforeExpire <= 0) throw new ValidationException("Should expire at least after 1 month.");
            if (renewDateOptional.HasValue && renewDateOptional.Value.Date < DateTime.Today.Date)
                throw new ValidationException("Invalid renew date");

            RenewDate = renewDateOptional ?? DateTimeOffset.Now;
            ExpireDate = RenewDate.Value.AddMonths(monthsBeforeExpire);
            IsActive = true;
        }

        public bool HasUid() => !string.IsNullOrEmpty(Uid);

    }
}
