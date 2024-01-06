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
        IsActive = true;
    }
    
    public Client(string name, string industry) : this()
    {
        Name = name;
        Industry = industry;
    }
    
    
    #endregion

    #region Behaviors
    
    public Subscription AddSubscription(Guid billingPlanId, DateTimeOffset startDate,DateTimeOffset endDate, int cardLevel, int numberOfMonthsToExpire)
    {
        Subscription subscription =
            new(billingPlanId: billingPlanId,
                startDate: startDate, 
                endDate,
                cardLevel,
                numberOfMonthsToExpire);

        subscription.Description = $"Card Level: {cardLevel}, card expires after {numberOfMonthsToExpire} month(s)";
        
        _subscriptions.Add(subscription);
        return subscription;
    }

  #endregion
    
}
