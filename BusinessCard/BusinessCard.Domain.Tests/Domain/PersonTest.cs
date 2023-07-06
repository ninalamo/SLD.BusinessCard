using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Faker;
using Shouldly;

namespace BusinessCard.Tests.Domain
{
    public class PersonTest
    {
        [Fact]
        public void PersonShouldBeCreated()
        {
            Person person = new();
            person.SetName(Name.First(), Name.Last(), Name.Last(), Name.Suffix());
            person.SetContactDetails(Phone.Number(), Internet.Email(), Address.Country());
            person.SetSocialMedia(new[] { @"facebook.com/ulaga", @"linkedin.com/chrri", @"pinterest.com/my/pinterest" });
        }

        [Fact]
        public void PersonShouldBeCreatedWithACard()
        {
            Person person = new();
            person.Card.ShouldNotBeNull();
        }
        
        [Fact]
        public void PersonShouldAddSocialMedia()
        {
            Person person = new();
            person.Card.ShouldNotBeNull();
        }
        
        [Fact]
        public void PersonShouldBeAbleToLinkEmptyCard()
        {
            Person person = new();
            person.SetCard("abc");

            person.Card.Key.ShouldBe("abc");
        }
        
        [Fact]
        public void PersonShouldBeAbleToLinkToAnotherCard()
        {
            Person person = new();
            person.SetCard("abc");
            person.Card.Key.ShouldBe("abc");
            
            person.RemoveCard();
            person.Card.ShouldBeNull();

            person.SetCard("xyz");
            person.Card.ShouldNotBeNull();
            person.Card.Key.ShouldBe("xyz");
        }
        
        [Fact]
        public void PersonShouldBeCheckedBeforeSaving()
        {
            Person person = new();
            person.HasCard().ShouldBeTrue();
            
            person.RemoveCard();
            person.HasCard().ShouldBeFalse();
        }
        
        [Fact]
        public void PersonShouldAllowDisableOfCard()
        {
            Person person = new();
            person.HasCard().ShouldBeTrue();
            person.DisableCard();
            person.Card.IsActive.ShouldBeFalse();
            
            person.EnableCard();
            person.Card.IsActive.ShouldBeTrue();

        }
    }
}
