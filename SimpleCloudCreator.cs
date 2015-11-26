using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Configuration;
using Nuclex.Game.Packing;
using XnaPoint = Microsoft.Xna.Framework.Point;


namespace _03_design_hw
{
    class SimpleCloudCreator : ICloudCreator
    {
        private const int MaxImageSize = 5000;
        private Color[] Colors{ get; }
        private RectanglePacker Packer{ get; }
        private int MinCount { get; }
        private int MaxCount { get; }
        private TagCloudSettings Settings { get; }
        private int RightBound {get; set; }
        private int BottomBound {get; set; }
        public IEnumerable<Word> Words { get; }
        public SimpleCloudCreator(TagCloudSettings settings, Statistic statistic)
        {
            Settings = settings;
            Colors = Settings.Colors;
            Packer = new ArevaloRectanglePacker(int.MaxValue, int.MaxValue);
            MinCount = statistic.MinCount;
            MaxCount = statistic.MaxCount;
            Words = statistic.WordsWithFrequency;
        }
        private Color GetRandomColor() => Colors[Settings.Random.Next(Colors.Length - 1)];

        private static SizeF GetWordRectangleSize(string text, Font font)
        {
            using (Image tempImage = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(tempImage))
            {
                var size = g.MeasureString(text, font);
                return new SizeF(size.Width*0.9f, size.Height*0.85f);
            }
        }
        public Font GetFont(Word word)
        {
            var size = Settings.MaxFontSize*(word.Frequency - MinCount)/(MaxCount - MinCount);
            size = size < Settings.MinFontSize? size + Settings.MinFontSize : size;
            return new Font(Settings.FontName, size);
        }
        private XnaPoint GetWordLocation(Word word, Font font, SizeF rectangleSize)
        {
            XnaPoint rectangleLocation;
            Packer.TryPack((int)rectangleSize.Width, (int)rectangleSize.Height, out rectangleLocation);
            return rectangleLocation;
        }

        private void UpdateBounds(SizeF rectangleSize, XnaPoint rectangleLocation)
        {
            if (rectangleSize.Width + rectangleLocation.X > RightBound)
                RightBound = rectangleLocation.X + (int)rectangleSize.Width;
            if (rectangleSize.Height + rectangleLocation.Y > BottomBound)
                BottomBound = rectangleLocation.Y + (int)rectangleSize.Height;
        }

        private Image GeneratePreReleaseImage()
        {
            var img = new Bitmap(MaxImageSize, MaxImageSize);
            using (Graphics graphics = Graphics.FromImage(img))
            {
                foreach (var word in Words)
                {
                    var font = GetFont(word);
                    var rectangleSize = GetWordRectangleSize(word.WordString, font);
                    var location = GetWordLocation(word, font, rectangleSize);
                    UpdateBounds(rectangleSize, location);
                    var color = GetRandomColor();
                    graphics.DrawString(word.WordString, font, new SolidBrush(color), location.X, location.Y);
                }
                return img;
            }
        }

        public Image GenerateReleaseImage(IEnumerable<Word> words)
        {
            using (var preReleaseImage = GeneratePreReleaseImage())
            {
                var releaseImage = new Bitmap(RightBound, BottomBound);
                using (Graphics releaseGraphics = Graphics.FromImage(releaseImage))
                {
                    releaseGraphics.Clear(Color.White);
                    releaseGraphics.DrawImage(preReleaseImage, 0, 0);
                    return releaseImage;
                }
            }
        }
    }
}
