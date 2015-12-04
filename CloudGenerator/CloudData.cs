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
        public CloudData(ILoader loader, Statistic statistic, RectanglePacker packer)
        {
            _minCount = statistic.MinCount;
            _maxCount = statistic.MaxCount;
            Width = loader.Width;
            Height = loader.Height;
            Words = statistic.WordsWithFrequency;
            _loader = loader;
            _minFontSize = loader.MinFontSize;
            _maxFontSize = loader.MaxFontSize;
            _colors = loader.Colors;
            _fontName = loader.FontName;
            _packer = packer;
            _random = new Random();
        }

        private readonly Random _random;
        
        public IEnumerable<Word> Words { get; }

        public int Height { get; }

        public int Width { get; }

        public int CurrentWidth { get; set; }

        public int CurrentHeight { get; set; }

        public Color RandomColor => _colors[_random.Next(_colors.Length - 1)];

        private readonly RectanglePacker _packer;

        private readonly ILoader _loader;

        private readonly string _fontName;

        private readonly Color[] _colors;

        private readonly int _maxFontSize;

        private readonly int _minFontSize;

        private readonly int _maxCount;

        private readonly int _minCount;

        private SizeF GetTagSize(Word word, Font font)
        {
            using (Image img = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(img))
                return g.MeasureString(word.WordString, font);
        }

        public Font GetFont(Word word)
        {
            var size = _maxFontSize * (word.Frequency - _minCount) / (_maxCount - _minCount);
            size = size < _minFontSize ? size + _minFontSize : size;
            return new Font(_fontName, size);
        }

        private Point GetWordLocation(SizeF rectangleSize)
        {
            Point rectangleLocation;
            _packer.TryPack((int)rectangleSize.Width, (int)rectangleSize.Height, out rectangleLocation);
            return rectangleLocation;
        }
        private int GetNewWidth(SizeF rectangleSize, Point location) =>
            Math.Max(CurrentWidth, location.X + (int)rectangleSize.Width);

        private int GetNewHeight(SizeF rectangleSize, Point location) =>
            Math.Max(CurrentHeight, location.Y + (int)rectangleSize.Height);
        
        public IEnumerable<Tag> GetTags()
        {
            foreach (var word in Words)
            {
                var font = GetFont(word);
                var rectangleSize = GetTagSize(word, font);
                var location = GetWordLocation(rectangleSize);
                var prevWidth = CurrentWidth;
                var prevHeight = CurrentHeight;
                var color = RandomColor;
                CurrentWidth = GetNewWidth(rectangleSize, location);
                CurrentHeight = GetNewHeight(rectangleSize, location);
                if (CurrentHeight > Height || CurrentWidth > Width)
                {
                    CurrentHeight = prevHeight;
                    CurrentWidth = prevWidth;
                    yield break;
                }
                yield return new Tag(word, location, font, color);
            }
        } 
    }
}
