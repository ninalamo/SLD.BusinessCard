using BusinessCard.Domain.AggregatesModel.CustomerAggregate;
using Moq;
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

        [Fact]
        public void shouldBeAbleToReplace()
        {
            var card = new NfcCard("Test", Guid.NewGuid());
            var oldId = card.CompanyId;

            card.Key.ShouldBe("Test");
            card.CompanyId.ShouldNotBe(default(Guid));

            card = new NfcCard("Another Test", Guid.NewGuid());
            card.Key.ShouldNotBe("Test");
            card.CompanyId.ShouldNotBe(oldId);
        }

    }
}