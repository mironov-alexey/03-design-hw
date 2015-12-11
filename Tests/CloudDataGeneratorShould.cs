using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Xna.Framework;
using Moq;
using Nuclex.Game.Packing;
using NUnit.Framework;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;
using Color = System.Drawing.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class CloudDataGeneratorShould
    {
        [SetUp]
        public void SetUp()
        {
            _wordsLoader = new Mock<IWordsLoader>();
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
            _fontCreator.Setup(x => x.GetFont(It.IsAny<IStatistic>(), It.IsAny<Word>())).Returns(new Font("Arial", 10));
            _wordsLoader.Setup(x => x.Words).Returns(new List<string> {"a", "b", "b", "c", "c", "c"});
            _statistic = new Mock<IStatistic>();
            _statistic.Setup(x => x.WordsWithFrequency).Returns(new List<Word> {new Word("a", 1), new Word("b", 2), new Word("c", 3)});
            _packer = new Mock<IPacker>();
            _data = new CloudData(_settings, _packer.Object,
                _fontCreator.Object);
        }

        private Settings _settings;
        private Mock<IWordsLoader> _wordsLoader;
        private Mock<IPacker> _packer;
        private Mock<IStatistic> _statistic;
        private CloudData _data;
        private Mock<IFontCreator> _fontCreator;

        [Test]
        public void Correctly_GetFont_WithMaxSize()
        {
            _fontCreator.Setup(x => x.GetFont(It.IsAny<IStatistic>(), It.IsAny<Word>())).Returns(new Font("Arial", 20));
            foreach (var tag in _data.GetTags(_statistic.Object))
                Assert.AreEqual(_fontCreator.Object.GetFont(_statistic.Object, tag.Word), tag.Font);
        }

        [Test]
        public void Correctly_GetFont_WithMinSize()
        {
            foreach (var tag in _data.GetTags(_statistic.Object))
                Assert.AreEqual(_fontCreator.Object.GetFont(_statistic.Object, tag.Word), tag.Font);
        }

        [Test]
        public void Correctly_GetTagsCount()
        {
            _settings.Width = 200;
            _settings.Height = 200;
            _data = new CloudData(_settings, _packer.Object,
                _fontCreator.Object);
            var tagsCount = _data.GetTags(_statistic.Object).Count();
            Assert.AreEqual(_wordsLoader.Object.Words.Distinct().Count(), tagsCount);
        }

        [Test]
        public void Correctly_GetWordInTags()
        {
            _settings.Width = 200;
            _settings.Height = 200;
            _data = new CloudData(_settings, _packer.Object,
                _fontCreator.Object);
            CollectionAssert.AreEquivalent(
                _statistic.Object.WordsWithFrequency.Select(w => w.WordString),
                _data.GetTags(_statistic.Object).Select(t => t.Word.WordString));
        }

        [Test]
        public void GenerateOnlyOneTagForWord()
        {
            CollectionAssert.AllItemsAreUnique(_data.GetTags(_statistic.Object).Select(t => t.Word.WordString));
        }

        [Test]
        public void TrimDoesntFitTags()
        {
            _packer.Setup(x => x.Pack(It.IsAny<int>(), It.IsAny<int>())).Returns(new Point(30, 30));
            var tagsCount = _data.GetTags(_statistic.Object).Count();
            Assert.Less(tagsCount, 3);
        }

        [Test]
        public void UpdateSize()
        {
            _settings.Width = 200;
            _settings.Height = 200;
            _data = new CloudData(_settings, _packer.Object, _fontCreator.Object);
            var currentSize = new SizeF(_data.CurrentWidth, _data.CurrentHeight);
            _data.GetTags(_statistic.Object).Count();
            var newSize = new SizeF(_data.CurrentWidth, _data.CurrentHeight);
            Assert.AreNotEqual(currentSize, newSize);
        }
    }
}