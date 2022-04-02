using System.Collections.Generic;
using System.Linq;

namespace Util
{
    public static class RandomGenerator
    {
        public static int GenerateRandomIntWithExcludingSet(int start, int count, HashSet<int> excludingSet)
        {
            var range = Enumerable.Range(start, count).Where(i => !excludingSet.Contains(i));

            var rand = new System.Random();
            int index = rand.Next(0, count - excludingSet.Count);
            return range.ElementAt(index);
        }
    }
}