using System.Collections.Generic;
using System.Linq;

namespace _03_design_hw
{
    public static class WordExtensions
    {
        public static IEnumerable<string> FilterBannedWords(this IEnumerable<string> words, HashSet<string> blackList) =>
            words.Where(w => !blackList.Contains(w));
    }
}