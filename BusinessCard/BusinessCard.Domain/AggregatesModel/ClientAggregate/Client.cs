namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public class Client : Entity, IAggregateRoot
{
    #region Attributes and Navigations

    public string Name { get; set; }
    public string Industry { get; set; }
    public bool IsBlackList { get; private set; }
  
    private readonly List<Subscription> _subscriptions;
    public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();

    #endregion
    
    #region Ctor  
    
    private Client()
    { 
        _subscriptions = new List<Subscription>();
    }
    
    public Client(string name, string industry) : this()
    {
        Name = name;
        Industry = industry;
    }
    
    
    #endregion

    #region Behaviors
    
    public Subscription AddSubscription(Guid billingPlanId, DateTimeOffset startDate, int numberOfMonthsToExpire)
    {
        Subscription subscription =
            new(billingPlanId: billingPlanId,
                startDate: startDate, 
                startDate.AddMonths(numberOfMonthsToExpire));
        
        _subscriptions.Add(subscription);
        return subscription;
    }

  #endregion
    
}
