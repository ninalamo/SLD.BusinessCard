using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Shouldly;

namespace BusinessCard.Domain.Tests.Domain;

public class SubscriptionTests
{
    [Fact]
    public void CanCreateSubscription()
    {
        Subscription subscription = new(
            billingPlanId: Guid.NewGuid(),
            startDate: DateTime.Today,
            numberOfMonthsToExpire: Faker.RandomNumber.Next(1, 12),
            cardSettingId: Guid.NewGuid());

        subscription.ShouldNotBeNull();
        subscription.IsTransient().ShouldBeTrue();
        subscription.State.ShouldBe(Subscription.Status.New);
        subscription.StartDate.ShouldNotBe(default);
        subscription.ActualEndDate.HasValue.ShouldBeFalse();
        subscription.StartDate.Date.ShouldBe(DateTime.Today.Date);
        subscription.EndDate.ShouldNotBe(default);
        subscription.Reason.ShouldNotBeEmpty();
        subscription.PaymentScheduleInterval.ShouldBe(Subscription.DefaultScheduleInterval);
        subscription.PaymentScheduleReminderInterval.ShouldBe(Subscription.DefaultReminderInterval);
        subscription.IsActive.ShouldBeTrue();
    }

    [Fact]
    public void CanCreateARenewalSubscription()
    {
        Subscription subscription = new(
            billingPlanId: Guid.NewGuid(),
            startDate: DateTime.Today,
            numberOfMonthsToExpire: Faker.RandomNumber.Next(1, 12),
            cardSettingId: Guid.NewGuid()); 
        
        
        var renewal = subscription.CreateRenewal(subscription.EndDate.AddMonths(  1), 12);
        renewal.GetCardSettingId().ShouldBe(subscription.GetCardSettingId());
        renewal.GetBillingPlanId().ShouldBe(subscription.GetBillingPlanId());
        renewal.StartDate.Date.ShouldBeGreaterThan(subscription.EndDate.Date);
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
        Subscription subscription = new(
            billingPlanId: Guid.NewGuid(),
            startDate: DateTime.Today,
            numberOfMonthsToExpire: Faker.RandomNumber.Next(1, 12),
            cardSettingId: Guid.NewGuid());

        subscription.Cancel(DateTime.Today, "Client withdrew.");
        subscription.ActualEndDate.HasValue.ShouldBeTrue();
        subscription.ActualEndDate.Value.Date.ShouldBe(DateTime.Today.Date);
        subscription.Reason.ShouldBe("Client withdrew.");
        subscription.State.ShouldBe(Subscription.Status.Cancelled);
        subscription.IsActive.ShouldBeFalse();
    }
    
    [Fact]
    public void CanPreTerminateASubscription()
    {
        Subscription subscription = new(
            billingPlanId: Guid.NewGuid(),
            startDate: DateTime.Today,
            numberOfMonthsToExpire: Faker.RandomNumber.Next(1, 12),
            cardSettingId: Guid.NewGuid());

        subscription.PreTerminate(DateTime.Today, "Breech of contract");
        subscription.ActualEndDate.HasValue.ShouldBeTrue();
        subscription.ActualEndDate.Value.Date.ShouldBe(DateTime.Today.Date);
        subscription.Reason.ShouldBe("Breech of contract");
        subscription.State.ShouldBe(Subscription.Status.PreTerminated);
        subscription.IsActive.ShouldBeFalse();
    }
    
    [Fact]
    public void CanEndContractASubscription()
    {
        Subscription subscription = new(
            billingPlanId: Guid.NewGuid(),
            startDate: DateTime.Today,
            numberOfMonthsToExpire: Faker.RandomNumber.Next(1, 12),
            cardSettingId: Guid.NewGuid());

        subscription.EndContract(DateTime.Today, "End of contract");
        subscription.ActualEndDate.HasValue.ShouldBeTrue();
        subscription.ActualEndDate.Value.Date.ShouldBe(DateTime.Today.Date);
        subscription.Reason.ShouldBe("End of contract");
        subscription.State.ShouldBe(Subscription.Status.EndOfContract);
        subscription.IsActive.ShouldBeFalse();
        
        
    }
    
    [Fact]
    public void CanMarkASubscriptionAsDelete()
    {
        Subscription subscription = new(
            billingPlanId: Guid.NewGuid(),
            startDate: DateTime.Today,
            numberOfMonthsToExpire: Faker.RandomNumber.Next(1, 12),
            cardSettingId: Guid.NewGuid());

        subscription.MarkAsDeleted("wrong input.");
        subscription.IsActive.ShouldBeFalse();
        subscription.State.ShouldBe(Subscription.Status.Deleted);
    }
    
    [Fact]
    public void CanChangeBillingPlanAndCardSetting()
    {
        Subscription subscription = new(
            billingPlanId: Guid.NewGuid(),
            startDate: DateTime.Today,
            numberOfMonthsToExpire: Faker.RandomNumber.Next(1, 12),
            cardSettingId: Guid.NewGuid());

        var oldBillingId = subscription.GetBillingPlanId();
        subscription.ChangeBillingPlan(Guid.NewGuid());

        var oldCardSettingId = subscription.GetCardSettingId();
        subscription.ChangeCardSetting(Guid.NewGuid());

        subscription.GetBillingPlanId().ShouldNotBe(oldBillingId);
        subscription.GetCardSettingId().ShouldNotBe(oldCardSettingId);

        var status = subscription.State;
        status.ShouldBe(subscription.State);
    }
}