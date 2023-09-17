using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Shouldly;

namespace BusinessCard.Domain.Tests.Domain;

public class CardSettingTests
{
    [Fact]
    public void CanCreateCardSetting()
    {
        CardSetting setting = new CardSetting(Guid.NewGuid(), "Level 1", 12);

        setting.Name.ShouldNotBe(default);
        setting.Name.ShouldBe("Level 1");
        var description = setting.Name;
        description.ShouldBeEquivalentTo(setting.Name);
        
        setting.Level.ShouldNotBe(default);
        setting.Level.ShouldBe(12);
        var level = setting.Level;
        level.ShouldBeEquivalentTo(setting.Level);
        

        CardSetting newSetting = setting;
        (newSetting == setting).ShouldBeTrue();
        newSetting.Name.ShouldBe(setting.Name);
        
    }
}