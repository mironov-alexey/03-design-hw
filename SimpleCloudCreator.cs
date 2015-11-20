using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Mapper;
using NHunspell;

namespace _03_design_hw
{
    class SimpleCloudCreator : ICloudCreator
    {
        private Dictionary<string, string> SpellingDictionaries{ get; }
        private List<Word> Words{ get; }
        private Color[] Colors{ get; }
        private HashSet<string> BlackList{ get; }
        private Random Random{ get; }
        private Canvas Canvas { get; }
        private int MinCount { get; }
        private int MaxCount { get; }
        private int TotalWordsCount { get; }
        private TagCloudSettings Settings { get; }
        public SimpleCloudCreator(BaseLoader dataLoader)
        {
            Settings = dataLoader.GetSettings();
            Random = new Random();
            SpellingDictionaries = dataLoader.GetSpellingDictionaries();
            OutputPath = dataLoader.GetOutputPath();
            var words = dataLoader.GetWords();
            BlackList = Settings.BlackList;
            Colors = Settings.Colors;
            TotalWordsCount = words.Count();
            Words = GetFrequency(words);
            Words = Settings.Top > 0 ? Words.Take(Settings.Top).ToList() : Words;
            MaxCount = Words.Max(w => w.Frequency);
            MinCount = Words.Min(w => w.Frequency);
            Canvas = new Canvas();
            Canvas.SetCanvasDimensions(Settings.Size.Width, Settings.Size.Height);
        }

        public string OutputPath{ get;}

        private List<Word> GetFrequency(IEnumerable<string> words)
        {
            return GetNormalizedWords(words)
                .GroupBy(w => w)
                .OrderByDescending(g => g.Count())
                .Select(g => new Word(g.First(), g.Count()))
                .ToList();
        }

        private IEnumerable<string> GetNormalizedWords(IEnumerable<string> words)
        {
            foreach (KeyValuePair<string, string> affixDictionary in SpellingDictionaries)
                using (var hunspell = new Hunspell(affixDictionary.Key, affixDictionary.Value))
//                    foreach (var word in words.SelectMany(word => hunspell.Stem(word)))
                    foreach (var word in words
                        .Where(IsNotInBlackList)
                        .Select(word =>
                    {
                        var stemmedWords = hunspell.Stem(word); // ?
                        if (stemmedWords.Count > 0)
                            return stemmedWords[0];
                        return word;
                    }))
                        yield return word;
        }

        private bool IsNotInBlackList(string word)
        {
            return !BlackList.Contains(word);
        }
        
        private Color GetRandomColor()
        {
            return Colors[Random.Next(Colors.Length - 1)];
        }

        private void DrawWord(Graphics graphics, Word word, Point location, Font font, Color color, bool isVertical=false)
        {
//            graphics.RotateTransform(isVertical ? 90 : 0); 
            graphics.DrawString(word.WordString, font, new SolidBrush(color), location.X, location.Y);
//            graphics.RotateTransform(isVertical ? 90 : 0);
        }

        private SizeF GetWordRectangleSize(string text, Font font)
        {
            using (Image tempImage = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(tempImage))
                return g.MeasureString(text, font);
        }

        private Font GetFont(Word word)
        {
//            var constant = Math.Log(MaxCount - (MinCount - 1)/(Settings.MaxFontSize - Settings.MinFontSize));
//            var size = (float)(Math.Log(word.Frequency - (MinCount - 1))/constant + Settings.MinFontSize);
            var size = (Settings.MaxFontSize*(word.Frequency - MinCount)/(MaxCount - MinCount));
            size = size == 0 ? size + 20 : size;
            Console.WriteLine("Word: " + word.WordString + " Count: " + word.Frequency);
            Console.WriteLine(size);
            Console.WriteLine(Words.Count);
            return new Font(Settings.FontName, size);
        }
        private Point GetWordLocation(Word word, Font font)
        {
            var rectangleSize = GetWordRectangleSize(word.WordString, font);
            int rectangleXOffset;
            int rectangleYOffset;
            int lowestFreeHeightDeficit;
            Canvas.AddRectangle(
                (int) rectangleSize.Width, 
                (int) rectangleSize.Height,
                out rectangleXOffset,
                out rectangleYOffset,
                out lowestFreeHeightDeficit
                );
            return new Point(rectangleXOffset, rectangleYOffset);
        }
        public void DrawAndSaveCloudImage()
        {
            using (Image img = new Bitmap(Settings.Size.Width, Settings.Size.Height))
            using (Graphics graphics = Graphics.FromImage(img))
            {
                foreach (var word in Words)
                {
                    var font = GetFont(word);
                    Point location = GetWordLocation(word, font);
                    var color = GetRandomColor();
                    DrawWord(graphics, word, location, font, color);
                }
                img.Save(OutputPath, ImageFormat.Png);
            }
        }
    }
}
