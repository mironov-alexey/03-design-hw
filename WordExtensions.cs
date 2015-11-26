using System.Collections.Generic;
using System.Linq;

namespace _03_design_hw
{
    public static class WordExtensions
    {
        public static bool IsNotInBlackList(this string word, HashSet<string> blackList) =>
            !blackList.Contains(word);

        public static IEnumerable<string> FilterBannedWords(this IEnumerable<string> words, HashSet<string> blackList) =>
            words.Where(w => IsNotInBlackList(w, blackList));
    }
}