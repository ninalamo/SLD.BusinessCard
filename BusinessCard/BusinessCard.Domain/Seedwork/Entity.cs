public abstract class Entity : IAuditable
{

    private int? _requestedHashCode;

    public virtual Guid Id { get; protected set; }


    public bool IsTransient() => Id == default;


    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Entity item))
            return false;

        if (ReferenceEquals(this, item))
            return true;

        if (GetType() != item.GetType())
            return false;

        if (item.IsTransient() || IsTransient())
            return false;

        return item.Id == Id;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode =
                    Id.GetHashCode() ^
                    31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }

        return base.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity? right)
    {
        if (Equals(left, null))
            return Equals(right, null) ? true : false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    //implementation of IAuditable
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; private set; } = DateTimeOffset.Now;
    public string ModifiedBy { get; private set; } = string.Empty;
    public DateTimeOffset ModifiedOn { get; private set; } = DateTimeOffset.Now;
    public bool IsActive { get; set; } = true;

    public void AuditOnCreate(string user)
    {

        //fail-fast guard clause
        if (string.IsNullOrWhiteSpace(user))
            throw new ArgumentNullException(nameof(AuditOnCreate),
                new ArgumentNullException("user field should not be empty."));

        CreatedBy = user;
        ModifiedBy = user;

        var now = DateTimeOffset.Now;
        CreatedOn = now;
        ModifiedOn = now;
    }

    public void AuditOnUpdate(string user)
    {
        //fail-fast guard clause
        if (string.IsNullOrWhiteSpace(user))
            throw new ArgumentNullException(nameof(AuditOnUpdate),
                new ArgumentNullException("user field should not be empty."));

        ModifiedBy = user;
        ModifiedOn = DateTimeOffset.Now;

    }

    public void Deactivate() => IsActive = false;
}