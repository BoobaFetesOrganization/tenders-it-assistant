using KellermanSoftware.CompareNetObjects;

namespace GenAIChat.Infrastructure.Database.TableStorage.Extensions
{
    internal static class AreSameExtensions
    {
        private static CompareLogic GetShallowLogic(IEnumerable<string> membersToIgnore) => new()
        {
            Config = new()
            {
                IgnoreObjectTypes = true,
                MaxDifferences = 10,
                MembersToIgnore = [.. membersToIgnore]
            }
        };

        public static bool AreShallowSameAs<T>(this T expected, T actual, IEnumerable<string>? ignoredMembers = null)
        {
            List<string> membersToIgnore = [
                .. (ignoredMembers ?? []),
                .. ((expected ?? actual)?.DetectMembersButProperties() ?? [])
                ];

            var comparison = GetShallowLogic(membersToIgnore).Compare(expected, actual);
            return comparison.AreEqual;
        }
    }
}
