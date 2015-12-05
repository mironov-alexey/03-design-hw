using System.Collections.Generic;
using System.Linq;

namespace _03_design_hw.Statistics
{
    public class Statistic
    {
        public Statistic(IEnumerable<Word> words)
        {
            WordsWithFrequency = words.ToList();
            MaxCount = WordsWithFrequency.Max(w => w.Frequency);
            MinCount = WordsWithFrequency.Min(w => w.Frequency);
        }

        public virtual int MaxCount{ get; }
        public virtual int MinCount{ get; }

        public IReadOnlyList<Word> WordsWithFrequency{ get; }
    }
}