using System.Collections.Generic;
using System.Linq;

namespace _03_design_hw.Statistics
{
    public class Statistic
    {
        public Statistic(List<Word> words)
        {
            WordsWithFrequency = words;
        }

        public int MaxCount => WordsWithFrequency.Max(w => w.Frequency);
        public int MinCount => WordsWithFrequency.Min(w => w.Frequency);

        public List<Word> WordsWithFrequency{ get; }
    }
}