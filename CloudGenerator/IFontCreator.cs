using System.Drawing;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;

namespace _03_design_hw.CloudGenerator
{
    public interface IFontCreator
    {
        Font GetFont(Statistic statistic, Word word);
    }
}