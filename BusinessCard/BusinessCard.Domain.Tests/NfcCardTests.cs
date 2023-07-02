using BusinessCard.Domain.AggregatesModel.CustomerAggregate;
using Shouldly;

namespace BusinessCard.Domain.Tests
{
    public class NfcCardTests
    {
        [Fact]
        public void shouldBeAbleToCreate()
        {
            var card = new NfcCard("Test", Guid.NewGuid());
            card.Key.ShouldBe("Test");
            card.CompanyId.ShouldNotBe(default(Guid));
        }
    }
}