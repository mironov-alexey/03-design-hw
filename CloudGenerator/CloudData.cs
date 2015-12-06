using System;
using System.Collections.Generic;
using System.Drawing;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;
using Point = Microsoft.Xna.Framework.Point;

namespace _03_design_hw.CloudGenerator
{
    public class CloudData : ICloudData
    {
        private readonly Color[] _colors;
        private readonly IFontCreator _fontCreator;

        private readonly IPacker _packer;

        private readonly Random _random;

        private readonly Settings _settings;

        public CloudData(Settings settings, IPacker packer, IFontCreator fontCreator)
        {
            _settings = settings;
            _colors = settings.Colors;
            _packer = packer;
            _fontCreator = fontCreator;
            _random = new Random();
        }

        public int CurrentWidth{ get; private set; }

        public int CurrentHeight{ get; private set; }

        private Color RandomColor => _colors[_random.Next(_colors.Length - 1)];

        public IEnumerable<Tag> GetTags(Statistic statistic)
        {
            foreach (var word in statistic.WordsWithFrequency)
            {
                var font = _fontCreator.GetFont(statistic, word);
                var rectangleSize = GetTagSize(word, font);
                var location = GetWordLocation(rectangleSize);
                var prevWidth = CurrentWidth;
                var prevHeight = CurrentHeight;
                var color = RandomColor;
                CurrentWidth = GetNewWidth(rectangleSize, location);
                CurrentHeight = GetNewHeight(rectangleSize, location);
                if (CurrentHeight > _settings.Height || CurrentWidth > _settings.Width)
                {
                    CurrentHeight = prevHeight;
                    CurrentWidth = prevWidth;
                    yield break;
                }
                yield return new Tag(word, location, font, color);
            }
        }

        private SizeF GetTagSize(Word word, Font font)
        {
            using (Image img = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(img))
                return g.MeasureString(word.WordString, font);
        }

        private Point GetWordLocation(SizeF rectangleSize)
        {
            return _packer.Pack((int) rectangleSize.Width, (int) rectangleSize.Height);
        }

        private int GetNewWidth(SizeF rectangleSize, Point location) =>
            Math.Max(CurrentWidth, location.X + (int) rectangleSize.Width);

        private int GetNewHeight(SizeF rectangleSize, Point location) =>
            Math.Max(CurrentHeight, location.Y + (int) rectangleSize.Height);
    }
}