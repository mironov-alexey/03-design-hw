using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHunspell;
namespace _03_design_hw
{
    class SimpleCloudCreator : ICloudCreator
    {
        private readonly Dictionary<string, string> _affixDictionary;
        public Dictionary<Word, int> Frequency{ get; }
        public int Width{ get; }
        public int Height{ get; }

        public Image Image;

        public bool[,] UsedBools;
        public int TotalWordsCount{ get; }

        public SimpleCloudCreator(IEnumerable<string> words, Dictionary<string, string> affixDictionary, int width, int height)
        {
            
            _affixDictionary = affixDictionary;
            Width = width;
            Height = height;
            TotalWordsCount = words.Count();
            Frequency = GetFrequency(words);
        }

        private Dictionary<Word, int> GetFrequency(IEnumerable<string> words)
        {
            return GetNormalizedWords(words)
                .GroupBy(w => w)
                .ToDictionary(g => new Word(g.First(), g.Count()), g => g.Count());
        }

        private IEnumerable<string> GetNormalizedWords(IEnumerable<string> words)
        {
            foreach (KeyValuePair<string, string> affixDictionary in _affixDictionary)
                using (var hunspell = new Hunspell(affixDictionary.Key, affixDictionary.Value))
//                    foreach (var word in words.SelectMany(word => hunspell.Stem(word)))
                    foreach (var word in words.Select(word =>
                    {
                        var stemmedWords = hunspell.Stem(word);
                        if (stemmedWords.Count > 0)
                            return stemmedWords[0];
                        return "";
                    }))
                        yield return word;
        }

        public Image Create()
        {
            using (Image img = new Bitmap(Width, Height))
            using (Graphics g = Graphics.FromImage(img))
            {
                foreach (var word in Frequency.Keys)
                {
                    Point location = GetWordLocation();
                    var fontSize = GetFontSize(word);
                    var font = new Font("Times New Roman", fontSize);
//                    var color = GetRandomColor();
                    var color = Color.Red;
                    DrawWord(g, word, location, font, color);
                }
            }
            throw new NotImplementedException();
        }

        private void DrawWord(Graphics graphics, Word word, Point location, Font font, Color color, bool isVertical=false)
        {
            graphics.RotateTransform(isVertical ? 90 : 0); 
            graphics.DrawString(word.WordString, font, new SolidBrush(color), location.X, location.Y);
            graphics.RotateTransform(isVertical ? 90 : 0);
        }

        private SizeF GetTextSize(string text, Font font)
        {
            using (Image tempImage = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(tempImage))
                return g.MeasureString(text, font);
//                return g.MeasureString(text, new Font("Times New Roman", 20f));
        }

        private int GetFontSize(Word word)
        {
            throw new NotImplementedException();
        }
        private Point GetWordLocation()
        {
            throw new NotImplementedException();
        }
    }
}
