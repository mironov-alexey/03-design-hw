using System.Collections.Generic;
using System.Linq;

namespace _03_design_hw.Statistics
{
    public class Statistic
    {
        public Statistic(List<Word> words)
        {
            WordsWithFrequency = words;
            MaxCount = WordsWithFrequency.Max(w => w.Frequency);
            MinCount = WordsWithFrequency.Min(w => w.Frequency);
        }

        public int MaxCount{ get; }
        public int MinCount{ get; }

        public List<Word> WordsWithFrequency{ get; }
    }
}