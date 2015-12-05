using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Moq;
using Nuclex.Game.Packing;
using NUnit.Framework;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class CloudDataGeneratorShould
    {
        [SetUp]
        public void SetUp()
        {
            _wordsLoader = new Mock<IWordsLoader>();
            _blackListLoader = new Mock<IBlackListLoader>();
            _blackListLoader.Setup(x => x.BlackList).Returns(new HashSet<string>());
            _settings = new Settings
            {
                Colors = new[] {Color.DarkRed, Color.Black, Color.Coral},
                FontName = "Arial",
                Height = 30,
                Width = 30,
                MaxFontSize = 20,
                MinFontSize = 10,
                SpellingDictionaries = new Dictionary<string, string>(),
                TagsCount = 3
            };
            _fontCreator = new FontCreator(_settings);
            _wordsLoader.Setup(x => x.Words).Returns(new List<string> {"a", "b", "b", "c", "c", "c"});
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, _statistic, new ArevaloRectanglePacker(int.MaxValue, int.MaxValue),
                _fontCreator);
        }

        private Settings _settings;
        private Mock<IWordsLoader> _wordsLoader;
        private Mock<IBlackListLoader> _blackListLoader;
        private Statistic _statistic;
        private CloudData _data;
        private IFontCreator _fontCreator;

        [Test]
        public void Correctly_GetFont_WithMaxSize()
        {
            var orderedWords = _statistic.WordsWithFrequency.OrderByDescending(w => w.Frequency).ToList();
            var actualFont = _fontCreator.GetFont( _statistic, orderedWords[0]);
            Assert.AreEqual(new Font("Arial", 20), actualFont);
        }

        [Test]
        public void Correctly_GetFont_WithMinSize()
        {
            var orderedWords = _statistic.WordsWithFrequency.OrderByDescending(w => w.Frequency).ToList();
            var actualFont = _fontCreator.GetFont(_statistic, orderedWords[2]);
            Assert.AreEqual(new Font("Arial", 10), actualFont);
        }

        [Test]
        public void Correctly_GetTagsCount()
        {
            _settings.Width = 200;
            _settings.Height = 200;
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, _statistic, new ArevaloRectanglePacker(int.MaxValue, int.MaxValue),
                _fontCreator);
            var tagsCount = _data.GetTags().Count();
            Assert.AreEqual(_wordsLoader.Object.Words.Distinct().Count(), tagsCount);
        }

        [Test]
        public void Correctly_GetTopTags()
        {
            _settings.TagsCount = 2;
            _settings.Width = 200;
            _settings.Height = 200;
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, _statistic, new ArevaloRectanglePacker(int.MaxValue, int.MaxValue),
                _fontCreator);
            var tagsCount = _data.GetTags().Count();
            Assert.AreEqual(_settings.TagsCount, tagsCount);
        }

        [Test]
        public void Correctly_GetWordInTags()
        {
            _settings.Width = 200;
            _settings.Height = 200;
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, _statistic, new ArevaloRectanglePacker(int.MaxValue, int.MaxValue),
                _fontCreator);
            CollectionAssert.AreEquivalent(
                _statistic.WordsWithFrequency.Select(w => w.WordString),
                _data.GetTags().Select(t => t.Word.WordString));
        }

        [Test]
        public void GenerateOnlyOneTagForWord()
        {
            CollectionAssert.AllItemsAreUnique(_data.GetTags().Select(t => t.Word.WordString));
        }

        [Test]
        public void TrimDoesntFitTags()
        {
            var tagsCount = _data.GetTags().Count();
            Assert.Less(tagsCount, 3);
        }

        [Test]
        public void UpdateSize()
        {
            _settings.Width = 200;
            _settings.Height = 200;
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, _statistic, new ArevaloRectanglePacker(int.MaxValue, int.MaxValue),
                _fontCreator);
            var currentSize = new SizeF(_data.CurrentWidth, _data.CurrentHeight);
            _data.GetTags().Count();
            var newSize = new SizeF(_data.CurrentWidth, _data.CurrentHeight);
            Assert.AreNotEqual(currentSize, newSize);
        }
    }
}