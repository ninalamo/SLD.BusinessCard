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
            person.SetSocialMedia(new[] { "facebook.com/ulaga", "linkedin.com/chrri", "pinterest.com/my/pinterest" });
        }

        [Fact]
        public void PersonShouldBeCreatedWithACard()
        {
            Person person = new();
            person.IsTransient().ShouldBeFalse();
            person.Card.ShouldNotBeNull();
        }
    }
}
