using System.Text.Json;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Faker;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Shouldly;

namespace BusinessCard.Tests.Domain
{
    public class PersonTest
    {
        private record SocialMediaObject
        {
            public string Facebook { get; init; }
            public string LinkedIn { get; init; }
            public string Instagram { get; init; }
            public string Pinterest { get; init; }
            public string Twitter { get; init; }
        }
        
        [Fact]
        public void PersonShouldBeCreated()
        {
            Person person = new();
            person.SetName(Name.First(), Name.Last(), Name.Last(), Name.Suffix());
            person.SetContactDetails(Phone.Number(), Internet.Email(), Address.Country());

            var json = new SocialMediaObject()
            {
                Facebook = "facebook.com",
                LinkedIn = "Linkedin.com",
                Instagram = "instagram.com",
                Pinterest = "pinterest.com",
                Twitter = "twitter.com",
            };
            person.SetSocialMedia(JsonSerializer.Serialize(json));

            person.FirstName.ShouldNotBeNull();
            person.LastName.ShouldNotBeNull();
            person.Address.ShouldNotBeNull();
            person.Email.ShouldNotBeNull();
            person.PhoneNumber.ShouldNotBeNull();

            var obj = JsonSerializer.Deserialize<SocialMediaObject>(person.SocialMedia);
            obj.ShouldBeEquivalentTo(json);

            obj.ShouldBeOfType<SocialMediaObject>();
            obj.Facebook.ShouldBeEquivalentTo(json.Facebook);
        }

        [Fact]
        public void PersonShouldBeCreatedWithACard()
        {
            Person person = new();
            person.Card.ShouldNotBeNull();
            person.Card.Key.ShouldBeEmpty();
        }
        
        
        [Fact]
        public void PersonShouldBeAbleToLinkEmptyCard()
        {
            Person person = new();
            person.RegisterCard("abc");

            person.Card.Key.ShouldBe("abc");
        }
        
        [Fact]
        public void PersonShouldBeAbleToLinkToAnotherCard()
        {
            Person person = new();
            person.RegisterCard("abc");
            person.Card.Key.ShouldBe("abc");
            
            person.RemoveCard();
            person.Card.ShouldBeNull();

            person.RegisterCard("xyz");
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
