using System;
using System.Collections.Generic;
using System.Drawing;
using Nuclex.Game.Packing;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;
using Point = Microsoft.Xna.Framework.Point;

namespace _03_design_hw.CloudGenerator
{
    public class CloudData : ICloudData
    {
        private readonly Color[] _colors;
        private readonly IFontCreator _fontCreator;

        private readonly RectanglePacker _packer;

        private readonly Random _random;

        private readonly Settings _settings;

        private readonly Statistic _statistic;

        private readonly IEnumerable<Word> _words;

        public CloudData(Settings settings, Statistic statistic, RectanglePacker packer, IFontCreator fontCreator)
        {
            _words = statistic.WordsWithFrequency;
            _settings = settings;
            _statistic = statistic;
            _colors = settings.Colors;
            _packer = packer;
            _fontCreator = fontCreator;
            _random = new Random();
        }


        public int CurrentWidth{ get; private set; }

        public int CurrentHeight{ get; private set; }

        private Color RandomColor => _colors[_random.Next(_colors.Length - 1)];

        public IEnumerable<Tag> GetTags()
        {
            foreach (var word in _words)
            {
                var font = _fontCreator.GetFont(_settings, _statistic, word);
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
            Point rectangleLocation;
            _packer.TryPack((int) rectangleSize.Width, (int) rectangleSize.Height, out rectangleLocation);
            return rectangleLocation;
        }

        private int GetNewWidth(SizeF rectangleSize, Point location) =>
            Math.Max(CurrentWidth, location.X + (int) rectangleSize.Width);

        private int GetNewHeight(SizeF rectangleSize, Point location) =>
            Math.Max(CurrentHeight, location.Y + (int) rectangleSize.Height);
    }
}