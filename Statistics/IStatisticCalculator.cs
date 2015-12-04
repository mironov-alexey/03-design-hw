using System.Collections.Generic;

namespace _03_design_hw.Statistics
{
    public interface IStatisticCalculator
    {
        Statistic Calculate(IEnumerable<string> words);
    }
}