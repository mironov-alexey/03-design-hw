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
    public class SimpleCloudGenerator : ICloudGenerator
    {
        private const int MaxImageSize = 5000;
        private RectanglePacker Packer{ get; }
        private ILoader Loader { get; }
        public Statistic Statistic{ get; }
        protected internal int CurrentWidth {get; set; }
        protected internal int CurrentHeight {get; set; }
        public IEnumerable<Word> Words { get; }
        public SimpleCloudGenerator(ILoader loader, Statistic statistic)
        {

            Loader = loader;
            Statistic = statistic;
            MinCount = statistic.MinCount;
            MaxCount = statistic.MaxCount;
            MinFontSize = loader.MinFontSize;
            MaxFontSize = loader.MaxFontSize;
            Colors = loader.Colors;
            FontName = loader.FontName;
            BlackList = loader.BlackList;
            Width = loader.Width;
            Height = loader.Height;
            Packer = new ArevaloRectanglePacker(int.MaxValue, int.MaxValue);
            Words = statistic.WordsWithFrequency;
        }

        private int Height{ get; }

        private int Width{ get; }

        private string FontName{ get; }

        private HashSet<string> BlackList{ get; }

        private Color[] Colors{ get; }

        private int MaxFontSize{ get; }

        private int MinFontSize{ get; }

        private int MaxCount{ get; }

        private int MinCount{ get; }

        protected internal Color GetRandomColor() => Colors[Loader.Random.Next(Colors.Length - 1)];

        protected internal Font GetFont(Word word)
        {
            var size = MaxFontSize*(word.Frequency - MinCount)/(MaxCount - MinCount);
            size = size < MinFontSize? size + MinFontSize : size;
            return new Font(FontName, size);
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


        protected internal Image GeneratePreReleaseImage()
        {
            var img = new Bitmap(MaxImageSize, MaxImageSize);
            using (Graphics graphics = Graphics.FromImage(img))
            {
                foreach (var word in Words)
                {
                    var font = GetFont(word);
                    var rectangleSize = graphics.MeasureString(word.WordString, font);
                    var location = GetWordLocation(rectangleSize);
                    var prevWidth = CurrentWidth;
                    var prevHeight = CurrentHeight;
                    CurrentWidth = GetNewWidth(rectangleSize, location);
                    CurrentHeight = GetNewHeight(rectangleSize, location);
                    if (CurrentHeight > Height || CurrentWidth > Width)
                    {
                        CurrentHeight = prevHeight;
                        CurrentWidth = prevWidth;
                        return img;
                    }
                    var color = GetRandomColor();
                    graphics.DrawString(word.WordString, font, new SolidBrush(color), location.X, location.Y);
                }
                return img;
            }
        }

        public Image GenerateCloudImage()
        {
            using (var preReleaseImage = GeneratePreReleaseImage())
            {
                var releaseImage = new Bitmap(Width, Height);
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
