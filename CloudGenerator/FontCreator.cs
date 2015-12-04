using System.Drawing;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;

namespace _03_design_hw.CloudGenerator
{
    public class FontCreator : IFontCreator
    {
        public Font GetFont(Settings settings, Statistic statistic, Word word)
        {
            var size = settings.MaxFontSize*(word.Frequency - statistic.MinCount)/
                       (statistic.MaxCount - statistic.MinCount);
            size = size < settings.MinFontSize ? size + settings.MinFontSize : size;
            return new Font(settings.FontName, size);
        }
    }
}