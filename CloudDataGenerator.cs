using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuclex.Game.Packing;
using Point = Microsoft.Xna.Framework.Point;

namespace _03_design_hw
{
    public class CloudDataGenerator
    {
        public CloudDataGenerator(ILoader loader, Statistic statistic)
        {

            _loader = loader;
            _statistic = statistic;
            MinCount = statistic.MinCount;
            MaxCount = statistic.MaxCount;
            MinFontSize = loader.MinFontSize;
            MaxFontSize = loader.MaxFontSize;
            Colors = loader.Colors;
            FontName = loader.FontName;
            BlackList = loader.BlackList;
            Width = loader.Width;
            Height = loader.Height;
            _packer = new ArevaloRectanglePacker(int.MaxValue, int.MaxValue);
            Words = statistic.WordsWithFrequency;
        }
        public IEnumerable<Word> Words { get; }
        private readonly RectanglePacker _packer;
        private Statistic _statistic;
        private readonly ILoader _loader;
        public int Height { get; }

        public int Width { get; }

        private string FontName { get; }

        private HashSet<string> BlackList { get; }

        private Color[] Colors { get; }

        private int MaxFontSize { get; }

        private int MinFontSize { get; }

        private int MaxCount { get; }

        private int MinCount { get; }

        public Color RandomColor => Colors[_loader.Random.Next(Colors.Length - 1)];

        public Font GetFont(Word word)
        {
            var size = MaxFontSize * (word.Frequency - MinCount) / (MaxCount - MinCount);
            size = size < MinFontSize ? size + MinFontSize : size;
            return new Font(FontName, size);
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
        public int CurrentWidth { get; set; }
        public int CurrentHeight { get; set; }

        private SizeF GetTagSize(Word word, Font font)
        {
            using (Image img = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(img))
                return g.MeasureString(word.WordString, font);
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
                yield return new Tag(word, location, rectangleSize, font, color);
            }
        } 
    }
}
