using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Shouldly;

namespace BusinessCard.Domain.Tests.Domain;

public class CardTest
{
    [Fact]
    public void CanCreateCard()
    {
        Card card = new();
        
        card.ShouldNotBeNull();
        card.HasUid().ShouldBeFalse();
        card.IsActive.ShouldBeFalse();
        card.Uid.ShouldBeEmpty();
        
        card.ActivatedDate.ShouldBeNull();
        card.RenewDate.ShouldBeNull();
        card.ExpireDate.ShouldBeNull();

    }
    
    [Theory]
    [InlineData("xxx",13)]
    [InlineData("adadfxxx",2)]
    [InlineData("xxadewrewfx",113)]
    [InlineData("xadsfgstgr34534fdsrxx",1213)]
    [InlineData("xxadsfadsfdasfx",33)]
    [InlineData("xadsfadsfxx",3)]
    [InlineData("xasdfadsxx",1)]
    public void CardCanBeActivated(string uid, int months)
    {
        Card card = new();
        card.Activate(uid,months);
        card.Uid.ShouldBe(uid);
        card.HasUid().ShouldBeTrue();
        card.IsActive.ShouldBeTrue();
        
        card.ActivatedDate.ShouldNotBeNull();
        card.ActivatedDate.Value.Date.ShouldBe(DateTime.Today.Date);

        card.ExpireDate.ShouldNotBeNull();
        card.ExpireDate.Value.Date.ShouldBe(DateTime.Today.AddMonths(months).Date);

        card.RenewDate.ShouldBeNull();
    }

    
    [Fact]
    public void CardCanBeDeactivated()
    {
        Card card = new();
        card.Activate("xxx", 13);
        card.Deactivate();

        card.IsActive.ShouldBeFalse();
        card.ExpireDate.Value.Date.ShouldBe(DateTime.Today.Date);
        card.RenewDate.ShouldBeNull();

    }
    
    [Fact]
    public void CardCanRenew()
    {
        Card card = new();
        card.Activate("xxx", 13);
        card.Deactivate();

        card.Renew(5, null);

        card.ExpireDate.ShouldNotBeNull();
        card.ExpireDate.Value.Date.ShouldBe(DateTime.Today.AddMonths(5).Date);
        card.RenewDate.Value.Date.ShouldBe(DateTime.Today.Date);
        
        card.Renew(1,DateTime.Today.AddMonths(4));
        card.ExpireDate.Value.Date.ShouldBe(DateTime.Today.AddMonths(5).Date);
        card.RenewDate.Value.Date.ShouldBe(DateTime.Today.AddMonths(4));
        card.IsActive.ShouldBeTrue();

    }
}