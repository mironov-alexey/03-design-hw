using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Configuration;
using Microsoft.Xna.Framework;
using Nuclex.Game.Packing;
using Color = System.Drawing.Color;
using XnaPoint = Microsoft.Xna.Framework.Point;


namespace _03_design_hw
{
    public class SimpleCloudCreator : ICloudCreator
    {
        private const int MaxImageSize = 5000;
        private RectanglePacker Packer{ get; }
        private BaseLoader Loader { get; }
        public Statistic Statistic{ get; }
        protected internal int CurrentWidth {get; set; }
        protected internal int CurrentHeight {get; set; }
        public IEnumerable<Word> Words { get; }
        public SimpleCloudCreator(BaseLoader loader, Statistic statistic)
        {
            Loader = loader;
            Statistic = statistic;
            Packer = new ArevaloRectanglePacker(int.MaxValue, int.MaxValue);
            Words = statistic.WordsWithFrequency;
        }
        protected internal Color GetRandomColor() => Loader.Colors[Loader.Random.Next(Loader.Colors.Length - 1)];

        protected internal static SizeF GetWordRectangleSize(string text, Font font)
        {
            using (Image tempImage = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(tempImage))
            {
                var size = g.MeasureString(text, font);
                return new SizeF(size.Width*0.9f, size.Height*0.85f);
            }
        }
        protected internal Font GetFont(Word word)
        {
            var size = Loader.MaxFontSize*(word.Frequency - Statistic.MinCount)/(Statistic.MaxCount - Statistic.MinCount);
            size = size < Loader.MinFontSize? size + Loader.MinFontSize : size;
            return new Font(Loader.FontName, size);
        }
        protected internal XnaPoint GetWordLocation(SizeF rectangleSize)
        {
            XnaPoint rectangleLocation;
            Packer.TryPack((int)rectangleSize.Width, (int)rectangleSize.Height, out rectangleLocation);
            return rectangleLocation;
        }
        protected internal int GetNewWidth(SizeF rectangleSize, XnaPoint location)
        {
            return Math.Max(CurrentWidth, location.X + (int) rectangleSize.Width);
        }

        protected internal int GetNewHeight(SizeF rectangleSize, XnaPoint location)
        {
            return Math.Max(CurrentHeight, location.Y + (int)rectangleSize.Height);
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
                    var location = GetWordLocation(rectangleSize);
                    CurrentWidth = GetNewWidth(rectangleSize, location);
                    CurrentHeight = GetNewHeight(rectangleSize, location);
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
                var releaseImage = new Bitmap(CurrentWidth, CurrentHeight);
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
