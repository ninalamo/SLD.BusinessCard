using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Shouldly;

namespace BusinessCard.Domain.Tests.Domain;

public class SubscriptionTests
{
    private readonly Subscription _subscription = new(
        billingPlanId: Guid.NewGuid(),
        startDate: DateTime.Today,
        DateTime.Today.AddMonths( Faker.RandomNumber.Next(1, 12))
    );

    [Fact]
    public void CanCreateSubscription()
    {

        _subscription.ShouldNotBeNull();
        _subscription.IsTransient().ShouldBeTrue();
        _subscription.State.ShouldBe(Subscription.Status.New);
        _subscription.StartDate.ShouldNotBe(default);
        _subscription.ActualEndDate.HasValue.ShouldBeFalse();
        _subscription.StartDate.Date.ShouldBe(DateTime.Today.Date);
        _subscription.EndDate.ShouldNotBe(default);
        _subscription.Reason.ShouldNotBeEmpty();
        _subscription.PaymentScheduleInterval.ShouldBe(Subscription.DefaultScheduleInterval);
        _subscription.PaymentScheduleReminderInterval.ShouldBe(Subscription.DefaultReminderInterval);
        _subscription.IsActive.ShouldBeTrue();
    }

    [Fact]
    public void CanCreateARenewalSubscription()
    {
        
        var renewal = _subscription.CreateRenewal(_subscription.EndDate.AddMonths(  1), 12);
     
        renewal.GetBillingPlanId().ShouldBe(_subscription.GetBillingPlanId());
        renewal.StartDate.Date.ShouldBeGreaterThan(_subscription.EndDate.Date);
        renewal.EndDate.ShouldBeGreaterThan(renewal.StartDate);
        renewal.State.ShouldBe(Subscription.Status.Renewed);

        renewal.UpdateReminderInterval(5);
        renewal.UpdatePaymentScheduleInterval(20);

        renewal.PaymentScheduleReminderInterval.ShouldBe(5);
        renewal.PaymentScheduleInterval.ShouldBe(20);
    }

    [Fact]
    public void CanCancelASubscription()
    {

        _subscription.Cancel(DateTime.Today, "Client withdrew.");
        _subscription.ActualEndDate.HasValue.ShouldBeTrue();
        _subscription.ActualEndDate.Value.Date.ShouldBe(DateTime.Today.Date);
        _subscription.Reason.ShouldBe("Client withdrew.");
        _subscription.State.ShouldBe(Subscription.Status.Cancelled);
        _subscription.IsActive.ShouldBeFalse();
    }
    
    [Fact]
    public void CanPreTerminateASubscription()
    {

        _subscription.PreTerminate(DateTime.Today, "Breech of contract");
        _subscription.ActualEndDate.HasValue.ShouldBeTrue();
        _subscription.ActualEndDate.Value.Date.ShouldBe(DateTime.Today.Date);
        _subscription.Reason.ShouldBe("Breech of contract");
        _subscription.State.ShouldBe(Subscription.Status.PreTerminated);
        _subscription.IsActive.ShouldBeFalse();
    }
    
    [Fact]
    public void CanEndContractASubscription()
    {
        _subscription.EndContract(DateTime.Today, "End of contract");
        _subscription.ActualEndDate.HasValue.ShouldBeTrue();
        _subscription.ActualEndDate.Value.Date.ShouldBe(DateTime.Today.Date);
        _subscription.Reason.ShouldBe("End of contract");
        _subscription.State.ShouldBe(Subscription.Status.EndOfContract);
        _subscription.IsActive.ShouldBeFalse();
    }
    
    [Fact]
    public void CanMarkASubscriptionAsDelete()
    {
        _subscription.MarkAsDeleted("wrong input.");
        _subscription.IsActive.ShouldBeFalse();
        _subscription.State.ShouldBe(Subscription.Status.Deleted);
    }
    
    [Fact]
    public void CanChangeBillingPlan()
    {
        var oldBillingId = _subscription.GetBillingPlanId();
        _subscription.ChangeBillingPlan(Guid.NewGuid());

        _subscription.GetBillingPlanId().ShouldNotBe(oldBillingId);
        
        var status = _subscription.State;
        status.ShouldBe(_subscription.State);
    }

    [Fact]
    public void CanSetCardSettings()
    {
        _subscription.ChangeCardSetting(1, 12, "only_a_level_one_with_twelve_month_period");
        _subscription.Setting.Level.ShouldBe(1);
        _subscription.Setting.ExpiresInMonths.ShouldBe(12);
        _subscription.Setting.Description.ShouldBe("only_a_level_one_with_twelve_month_period");
    }

    [Fact]
    public void CanGetIdAsName()
    {
        var name = _subscription.GetName();
        name.ShouldBe(_subscription.Id.ToString().Replace("-","").ToUpper());
    }
}