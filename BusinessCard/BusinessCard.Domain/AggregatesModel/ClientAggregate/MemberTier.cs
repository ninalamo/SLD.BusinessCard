using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{
    public class MemberTier : Enumeration
    {
        public int Level { get; private set; }
        
        public MemberTier(int level, Guid id, string name) : base(id, name)
        {
            Level = level;
        }

        public static MemberTier GetOne() => GetLevels().First(i => i.Level == 1);

        public static IEnumerable<MemberTier> GetLevels()
        {
            return new[]
            {
                new MemberTier(1,Guid.Parse("00000000-0000-0000-0000-000000000001"),"level_one"),
                new MemberTier(2,Guid.Parse("00000000-0000-0000-0000-000000000002"),"level_two"),
                new MemberTier(3,Guid.Parse("00000000-0000-0000-0000-000000000003"),"level_three"),
                new MemberTier(4,Guid.Parse("00000000-0000-0000-0000-000000000004"),"level_four"),
                new MemberTier(5,Guid.Parse("00000000-0000-0000-0000-000000000005"),"level_five"),
                new MemberTier(6,Guid.Parse("00000000-0000-0000-0000-000000000006"),"level_six"),
                new MemberTier(7,Guid.Parse("00000000-0000-0000-0000-000000000007"),"level_seven"),
            };
        }
    }
}
