using System.Text.Json;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Faker;
using Shouldly;

namespace BusinessCard.Domain.Tests.Domain
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
            person.Occupation = "N/A";
            

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
            person.MiddleName.ShouldNotBeNull();
            person.NameSuffix.ShouldNotBeNull();
            person.Address.ShouldNotBeNull();
            person.Email.ShouldNotBeNull();
            person.PhoneNumber.ShouldNotBeNull();
            person.IsActive.ShouldBeFalse();
            person.Occupation.ShouldNotBeNull();
            person.IsSubscriptionOverride.ShouldBeFalse();

            var obj = JsonSerializer.Deserialize<SocialMediaObject>(person.SocialMedia);
            obj.ShouldBeEquivalentTo(json);

            obj.ShouldBeOfType<SocialMediaObject>();
            obj.Facebook.ShouldBeEquivalentTo(json.Facebook);
        }

        [Fact]
        public void PersonShouldHaveEmptyCards()
        {
            Person person = new();
            person.Cards.ShouldNotBeNull();
            person.Cards.ShouldBeEmpty();
        }
        
        [Fact]
        public void PersonShouldBeCreateWithoutIdentity()
        {
            Person person = new();
            person.HasIdentity().ShouldBeFalse();
        }
        
        [Fact]
        public void PersonCanSetIdentity()
        {
            Person person = new();
            person.HasIdentity().ShouldBeFalse();
            person.SetIdentity(Guid.NewGuid().ToString());
        }
        
        [Fact]
        public void PersonCanValidateSetIdentity()
        {
            Person person = new();
            person.HasIdentity().ShouldBeFalse();
            Assert.Throws<FormatException>(() => person.SetIdentity(""));
        }
        
        [Fact]
        public void PersonCanSetSubscription()
        {
            Person person = new();
            Assert.Throws<NotImplementedException>(() => person.SetSubscription(0));
        }
        
        

    
    }
}
