using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.SubscriptionAggregate;

// public class Subscription : Entity
// {
//     public int Level { get; private set; }
//     public int ExpiresInMonths { get; private set; }
//         
//     public Subscription(int level, Guid id, int expiresInMonths)
//     {
//         Level = level;
//         
//     }
//
//     public static Subscription GetOne() => GetLevels().First(i => i.Level == 1);
//     public static Subscription GetTier(int level) => GetLevels().First(i => i.Level == level);
//
//     public static IEnumerable<Subscription> GetLevels()
//     {
//         return new[]
//         {
//             new Subscription(1,Guid.Parse("00000000-0000-0000-0000-000000000001"),"level_one"),
//             new Subscription(2,Guid.Parse("00000000-0000-0000-0000-000000000002"),"level_two"),
//             new Subscription(3,Guid.Parse("00000000-0000-0000-0000-000000000003"),"level_three"),
//             new Subscription(4,Guid.Parse("00000000-0000-0000-0000-000000000004"),"level_four"),
//             new Subscription(5,Guid.Parse("00000000-0000-0000-0000-000000000005"),"level_five"),
//             new Subscription(6,Guid.Parse("00000000-0000-0000-0000-000000000006"),"level_six"),
//             new Subscription(7,Guid.Parse("00000000-0000-0000-0000-000000000007"),"level_seven"),
//         };
//     }
// }