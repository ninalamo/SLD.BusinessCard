using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Shouldly;

namespace BusinessCard.Domain.Tests.Domain;

public class CardSettingTests
{
    [Fact]
    public void CanCreateCardSetting()
    {
        CardSetting setting = new CardSetting(1, "Level 1", 12);

        setting.Description.ShouldNotBe(default);
        setting.Description.ShouldBe("Level 1");
        var description = setting.Description;
        description.ShouldBeEquivalentTo(setting.Description);
        
        setting.Level.ShouldNotBe(default);
        setting.Level.ShouldBe(1);
        var level = setting.Level;
        level.ShouldBeEquivalentTo(setting.Level);

        setting.ExpiresInMonths.ShouldNotBe(default);
        setting.ExpiresInMonths.ShouldBe(12);
        var months = setting.ExpiresInMonths;
        months.ShouldBeEquivalentTo(setting.ExpiresInMonths);

        CardSetting newSetting = setting;
        (newSetting == setting).ShouldBeTrue();
        newSetting.Description.ShouldBe(setting.Description);
        
    }
}