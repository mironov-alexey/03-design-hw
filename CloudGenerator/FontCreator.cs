using System.Drawing;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;

namespace _03_design_hw.CloudGenerator
{
    public class FontCreator : IFontCreator
    {
        private readonly Settings _settings;

        public FontCreator(Settings settings)
        {
            _settings = settings;
        }

        public Font GetFont(Statistic statistic, Word word)
        {
            var size = _settings.MaxFontSize*(word.Frequency - statistic.MinCount)/
                       (statistic.MaxCount - statistic.MinCount);
            size = size < _settings.MinFontSize ? size + _settings.MinFontSize : size;
            return new Font(_settings.FontName, size);
        }
    }
}