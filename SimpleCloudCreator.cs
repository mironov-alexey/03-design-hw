using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using NHunspell;
using Nuclex.Game.Packing;


namespace _03_design_hw
{
    class SimpleCloudCreator : ICloudCreator
    {
        private const int MaxImageSize = 5000;

        private Dictionary<string, string> SpellingDictionaries{ get; }
        private List<Word> Words{ get; }
        private Color[] Colors{ get; }
        private HashSet<string> BlackList{ get; }
        private Random Random{ get; }
        private RectanglePacker Packer{ get; }
        private int MinCount { get; }
        private int MaxCount { get; }
        private TagCloudSettings Settings { get; }
        private int RightBound {get; set; }
        private int BottomBound {get; set; }
        public SimpleCloudCreator(BaseLoader dataLoader)
        {
            Settings = dataLoader.GetSettings();
            Random = new Random();
            SpellingDictionaries = dataLoader.GetSpellingDictionaries();
            OutputPath = dataLoader.GetOutputPath();
            var words = dataLoader.GetWords();
            BlackList = Settings.BlackList;
            Colors = Settings.Colors;
            Words = GetFrequency(words);
            Words = Settings.Top > 0 ? Words.Take(Settings.Top).ToList() : Words;
            MaxCount = Words.Max(w => w.Frequency);
            MinCount = Words.Min(w => w.Frequency);
            Packer = new ArevaloRectanglePacker(int.MaxValue, int.MaxValue);
        }

        public string OutputPath{ get;}

        private List<Word> GetFrequency(IEnumerable<string> words) => GetNormalizedWords(words)
            .GroupBy(w => w)
            .OrderByDescending(g => Random.Next())
            .Select(g => new Word(g.First(), g.Count()))
            .ToList();

        private IEnumerable<string> GetNormalizedWords(IEnumerable<string> words)
        {
            foreach (KeyValuePair<string, string> affixDictionary in SpellingDictionaries)
                using (var hunspell = new Hunspell(affixDictionary.Key, affixDictionary.Value))
                    foreach (var word in words.Where(IsNotInBlackList))
                    {
                        yield return word;
                    }
        }

        private bool IsNotInBlackList(string word) => !BlackList.Contains(word);
        
        private Color GetRandomColor() => Colors[Random.Next(Colors.Length - 1)];

        private void DrawWord(Graphics graphics, Word word, Point location, Font font, Color color, bool isVertical=false) =>
            graphics.DrawString(word.WordString, font, new SolidBrush(color), location.X, location.Y);

        private SizeF GetWordRectangleSize(string text, Font font)
        {
            using (Image tempImage = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(tempImage))
                return g.MeasureString(text, font);
        }

        private Font GetFont(Word word)
        {
/*          var constant = Math.Log(MaxCount - (MinCount - 1)/(Settings.MaxFontSize - Settings.MinFontSize));
            var size = (float)(Math.Log(word.Frequency - (MinCount - 1))/constant + Settings.MinFontSize);
            Логарифмическое определение размера шрифта
*/
            var size = (Settings.MaxFontSize*(word.Frequency - MinCount)/(MaxCount - MinCount));
            size = size < Settings.MinFontSize? size + Settings.MinFontSize : size;
            return new Font(Settings.FontName, size);
        }
        private Point GetWordLocation(Word word, Font font)
        {
            var rectangleSize = GetWordRectangleSize(word.WordString, font);
            Microsoft.Xna.Framework.Point rectangleLocation;
            Packer.TryPack((int) rectangleSize.Width, (int) rectangleSize.Height, out rectangleLocation);
            if (rectangleSize.Width + rectangleLocation.X > RightBound)
                RightBound = rectangleLocation.X + (int)rectangleSize.Width;
            if (rectangleSize.Height + rectangleLocation.Y > BottomBound)
                BottomBound = rectangleLocation.Y + (int) rectangleSize.Height;
            return new Point(rectangleLocation.X, rectangleLocation.Y);
        }
        public void GeneratePreReleaseImage()
        {
            using (Image img = new Bitmap(MaxImageSize, MaxImageSize))
            {
                using (Graphics graphics = Graphics.FromImage(img))
                {
                    foreach (var word in Words)
                    {
                        var font = GetFont(word);
                        Point location = GetWordLocation(word, font);
                        var color = GetRandomColor();
                        DrawWord(graphics, word, location, font, color);
                    }
                }
                DrawReleaseImage(img);
            }
        }

        private void DrawReleaseImage(Image img)
        {
            using (Image releaseImage = new Bitmap(RightBound, BottomBound))
            using (Graphics releaseGraphics = Graphics.FromImage(releaseImage))
            {
                releaseGraphics.Clear(Color.White);
                releaseGraphics.DrawImage(img, 0, 0);
                releaseImage.Save(OutputPath, ImageFormat.Png);
            }
        }
    }
}
