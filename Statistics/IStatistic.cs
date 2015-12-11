using System.Collections.Generic;

namespace _03_design_hw.Statistics
{
    public interface IStatistic
    {
        IReadOnlyList<Word> WordsWithFrequency{ get; }
        int MaxCount{ get; }
        int MinCount{ get; }
    }
}