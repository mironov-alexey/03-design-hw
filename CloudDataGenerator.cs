using System;
using System.Collections.Generic;
using System.Drawing;
using Nuclex.Game.Packing;
using _03_design_hw.Loaders;
using Point = Microsoft.Xna.Framework.Point;

namespace _03_design_hw
{
    public class CloudDataGenerator
    {
        public CloudDataGenerator(ILoader loader, Statistic.Statistic statistic)
        {
            MinCount = statistic.MinCount;
            MaxCount = statistic.MaxCount;
            Width = loader.Width;
            Height = loader.Height;
            Words = statistic.WordsWithFrequency;
            _loader = loader;
            _minFontSize = loader.MinFontSize;
            _maxFontSize = loader.MaxFontSize;
            _colors = loader.Colors;
            _fontName = loader.FontName;
            _packer = new ArevaloRectanglePacker(int.MaxValue, int.MaxValue);
        }
        public IEnumerable<Word.Word> Words { get; }
        public int Height { get; }
        public int Width { get; }
        public int CurrentWidth { get; set; }
        public int CurrentHeight { get; set; }
        public Color RandomColor => _colors[_loader.Random.Next(_colors.Length - 1)];
        private readonly RectanglePacker _packer;
        private readonly ILoader _loader;
        private readonly string _fontName;
        private readonly Color[] _colors;
        private readonly int _maxFontSize;
        private readonly int _minFontSize;
        private int MaxCount { get; }
        private int MinCount { get; }
        private SizeF GetTagSize(Word.Word word, Font font)
        {
            using (Image img = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(img))
                return g.MeasureString(word.WordString, font);
        }
        public Font GetFont(Word.Word word)
        {
            var size = _maxFontSize * (word.Frequency - MinCount) / (MaxCount - MinCount);
            size = size < _minFontSize ? size + _minFontSize : size;
            return new Font(_fontName, size);
        }
        public Point GetWordLocation(SizeF rectangleSize)
        {
            Point rectangleLocation;
            _packer.TryPack((int)rectangleSize.Width, (int)rectangleSize.Height, out rectangleLocation);
            return rectangleLocation;
        }
        public int GetNewWidth(SizeF rectangleSize, Point location)
        {
            return Math.Max(CurrentWidth, location.X + (int)rectangleSize.Width);
        }

        public int GetNewHeight(SizeF rectangleSize, Point location)
        {
            return Math.Max(CurrentHeight, location.Y + (int)rectangleSize.Height);
        }

        
        public IEnumerable<Tag> GetTagsSequence()
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
