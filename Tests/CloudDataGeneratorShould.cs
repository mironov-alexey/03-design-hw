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
            _fontCreator = new Mock<IFontCreator>();
            _fontCreator.Setup(x => x.GetFont(It.IsAny<Statistic>(), It.IsAny<Word>())).Returns(new Font("Arial", 10));
            _wordsLoader.Setup(x => x.Words).Returns(new List<string> {"a", "b", "b", "c", "c", "c"});
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, GetExternalPacker(),
                _fontCreator.Object);
        }

        private static ExternalPacker GetExternalPacker()
            => new ExternalPacker(new ArevaloRectanglePacker(int.MaxValue, int.MaxValue));

        private Settings _settings;
        private Mock<IWordsLoader> _wordsLoader;
        private Mock<IBlackListLoader> _blackListLoader;
        private Statistic _statistic;
        private CloudData _data;
        private Mock<IFontCreator> _fontCreator;

        [Test]
        public void Correctly_GetFont_WithMaxSize()
        {
            _fontCreator.Setup(x => x.GetFont(It.IsAny<Statistic>(), It.IsAny<Word>())).Returns(new Font("Arial", 20));
            foreach (var tag in _data.GetTags(_statistic))
                Assert.AreEqual(new Font("Arial", 20), tag.Font);
        }

        [Test]
        public void Correctly_GetFont_WithMinSize()
        {
            foreach (var tag in _data.GetTags(_statistic))
                Assert.AreEqual(new Font("Arial", 10), tag.Font);
        }

        [Test]
        public void Correctly_GetTagsCount()
        {
            _settings.Width = 200;
            _settings.Height = 200;
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, GetExternalPacker(),
                _fontCreator.Object);
            var tagsCount = _data.GetTags(_statistic).Count();
            Assert.AreEqual(_wordsLoader.Object.Words.Distinct().Count(), tagsCount);
        }

        [Test]
        public void Correctly_GetTopTags()
        {
            _settings.TagsCount = 2;
            _settings.Width = 200;
            _settings.Height = 200;
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, GetExternalPacker(),
                _fontCreator.Object);
            var tagsCount = _data.GetTags(_statistic).Count();
            Assert.AreEqual(_settings.TagsCount, tagsCount);
        }

        [Test]
        public void Correctly_GetWordInTags()
        {
            _settings.Width = 200;
            _settings.Height = 200;
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, GetExternalPacker(),
                _fontCreator.Object);
            CollectionAssert.AreEquivalent(
                _statistic.WordsWithFrequency.Select(w => w.WordString),
                _data.GetTags(_statistic).Select(t => t.Word.WordString));
        }

        [Test]
        public void GenerateOnlyOneTagForWord()
        {
            CollectionAssert.AllItemsAreUnique(_data.GetTags(_statistic).Select(t => t.Word.WordString));
        }

        [Test]
        public void TrimDoesntFitTags()
        {
            var tagsCount = _data.GetTags(_statistic).Count();
            Assert.Less(tagsCount, 3);
        }

        [Test]
        public void UpdateSize()
        {
            _settings.Width = 200;
            _settings.Height = 200;
            _statistic = new StatisticCalculator(_settings, _blackListLoader.Object).Calculate(_wordsLoader.Object.Words);
            _data = new CloudData(_settings, GetExternalPacker(), _fontCreator.Object);
            var currentSize = new SizeF(_data.CurrentWidth, _data.CurrentHeight);
            _data.GetTags(_statistic).Count();
            var newSize = new SizeF(_data.CurrentWidth, _data.CurrentHeight);
            Assert.AreNotEqual(currentSize, newSize);
        }
    }
}