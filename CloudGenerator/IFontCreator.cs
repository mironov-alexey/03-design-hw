using System.Drawing;
using _03_design_hw.Statistics;

namespace _03_design_hw.CloudGenerator
{
    public interface IFontCreator
    {
        Font GetFont(IStatistic statistic, Word word);
    }
}