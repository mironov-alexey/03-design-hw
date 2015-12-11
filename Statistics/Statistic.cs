using System.Collections.Generic;
using System.Linq;

namespace _03_design_hw.Statistics
{
    public class Statistic : IStatistic
    {
        public Statistic(IEnumerable<Word> words)
        {
            WordsWithFrequency = words.ToList();
            MaxCount = WordsWithFrequency.Max(w => w.Frequency);
            MinCount = WordsWithFrequency.Min(w => w.Frequency);
        }

        public int MaxCount{ get; }
        public int MinCount{ get; }

        public IReadOnlyList<Word> WordsWithFrequency{ get; }
    }
}