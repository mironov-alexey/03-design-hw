using System.Collections.Generic;
using System.Linq;

namespace _03_design_hw.CloudGenerator
{
    public static class WordExtensions
    {
        public static IEnumerable<string> FilterBannedWords(this IEnumerable<string> words, IWordsFilter filter)
            =>
                words.Where(filter.Filter);
    }
}