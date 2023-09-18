using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Shouldly;

namespace BusinessCard.Domain.Tests.Domain;

public class SocialMediaTests
{
    [Fact]
    public void CanCreateSocialMediaAccount()
    {
        SocialMedia accounts = new SocialMedia("", "", "", "", "");
        SocialMedia refAccount = accounts;
        
        accounts.ShouldNotBeNull();
        accounts.Facebook.ShouldNotBeNull();
        accounts.Instagram.ShouldNotBeNull();
        accounts.LinkedIn.ShouldNotBeNull();
        accounts.Pinterest.ShouldNotBeNull();
        accounts.Twitter.ShouldNotBeNull();

        refAccount.ShouldBe(accounts);


    }
}