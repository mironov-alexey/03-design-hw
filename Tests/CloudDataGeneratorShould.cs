using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Moq;
using NUnit.Framework;
using _03_design_hw.Loaders;
using _03_design_hw.Savers;
using Color = System.Drawing.Color;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class CloudDataGeneratorShould
    {
        private Mock<ILoader> _loader;
        private Statistic.Statistic _statistic;
        private CloudData _dataGenerator;

        [SetUp]
        public void SetUp()
        {
            _loader = new Mock<ILoader>();
            _loader.Setup(x => x.BlackList).Returns(new HashSet<string>());
            _loader.Setup(x => x.Top).Returns(3);
            _loader.Setup(x => x.Width).Returns(30);
            _loader.Setup(x => x.Height).Returns(30);
            _loader.Setup(x => x.Random).Returns(new Random());
            _loader.Setup(x => x.FontName).Returns("Arial");
            _loader.Setup(x => x.MaxFontSize).Returns(20);
            _loader.Setup(x => x.MinFontSize).Returns(10);
            _loader.Setup(x => x.Colors).Returns(new[] {Color.DarkRed, Color.Black, Color.Coral});
            _loader.Setup(x => x.Words).Returns(new List<string> {"a", "b", "b", "c", "c", "c"});
            _statistic = new Statistic.Statistic(_loader.Object);
            _dataGenerator = new CloudData(_loader.Object, _statistic);
        }

        [Test]
        public void Correctly_GetFont_WithMaxSize()
        {
            var orderedWords = _statistic.WordsWithFrequency.OrderByDescending(w => w.Frequency).ToList();
            var actualFont = _dataGenerator.GetFont(orderedWords[0]);
            Assert.AreEqual(new Font("Arial", 20), actualFont);
        }

        [Test]
        public void Correctly_GetFont_WithMinSize()
        {
            var orderedWords = _statistic.WordsWithFrequency.OrderByDescending(w => w.Frequency).ToList();
            var actualFont = _dataGenerator.GetFont(orderedWords[2]);
            Assert.AreEqual(new Font("Arial", 10), actualFont);
        }

        [Test]
        public void UpdateWidth()
        {
            _dataGenerator.CurrentWidth = _dataGenerator.GetNewWidth(new SizeF(10, 10), new XnaPoint(0, 0));
            _dataGenerator.CurrentWidth = _dataGenerator.GetNewWidth(new SizeF(10, 10), new XnaPoint(10, 0));
            Assert.AreEqual(_dataGenerator.CurrentWidth, 20);
        }

        [Test]
        public void UpdateHeight()
        {
            _dataGenerator.CurrentHeight = _dataGenerator.GetNewHeight(new SizeF(10, 10), new XnaPoint(0, 0));
            _dataGenerator.CurrentHeight = _dataGenerator.GetNewHeight(new SizeF(10, 10), new XnaPoint(0, 10));
            Assert.AreEqual(_dataGenerator.CurrentHeight, 20);
        }

        [Test]
        public void UpdateSize()
        {
            _loader.Setup(x => x.Width).Returns(200);
            _loader.Setup(x => x.Height).Returns(200);
            _statistic = new Statistic.Statistic(_loader.Object);
            _dataGenerator = new CloudData(_loader.Object, _statistic);
            var currentSize = new SizeF(_dataGenerator.CurrentWidth, _dataGenerator.CurrentHeight);
            _dataGenerator.GetTags().Count();
            var newSize = new SizeF(_dataGenerator.CurrentWidth, _dataGenerator.CurrentHeight);
            Assert.AreNotEqual(currentSize, newSize);
        }

        [Test]
        public void GetRandomColor()
        {
            for (var i = 0; i < 1000; i++)
            {
                var color = _dataGenerator.RandomColor;
                CollectionAssert.Contains(_loader.Object.Colors, color);
            }
        }

        [Test]
        public void Correctly_GetTagsCount()
        {
            _loader.Setup(x => x.Width).Returns(200);
            _loader.Setup(x => x.Height).Returns(200);
            _statistic = new Statistic.Statistic(_loader.Object);
            _dataGenerator = new CloudData(_loader.Object, _statistic);
            var tagsCount = _dataGenerator.GetTags().Count();
            Assert.AreEqual(_loader.Object.Words.Distinct().Count(), tagsCount);
        }

        [Test]
        public void Correctly_GetTopTags()
        {
            _loader.Setup(x => x.Top).Returns(2);
            _loader.Setup(x => x.Width).Returns(200);
            _loader.Setup(x => x.Height).Returns(200);
            _statistic = new Statistic.Statistic(_loader.Object);
            _dataGenerator = new CloudData(_loader.Object, _statistic);
            var tagsCount = _dataGenerator.GetTags().Count();
            Assert.AreEqual(_loader.Object.Top, tagsCount);
        }

        [Test]
        public void GenerateOnlyOneTagForWord()
        {
            CollectionAssert.AllItemsAreUnique(_dataGenerator.GetTags().Select(t => t.Word.WordString));
        }

        [Test]
        public void Correctly_GetWordInTags()
        {
            _loader.Setup(x => x.Width).Returns(200);
            _loader.Setup(x => x.Height).Returns(200);
            _statistic = new Statistic.Statistic(_loader.Object);
            _dataGenerator = new CloudData(_loader.Object, _statistic);
            CollectionAssert.AreEquivalent(
                _statistic.WordsWithFrequency.Select(w => w.WordString),
                _dataGenerator.GetTags().Select(t => t.Word.WordString));
        }

        [Test]
        public void TrimDoesntFitTags()
        {
            var tagsCount = _dataGenerator.GetTags().Count();
            Assert.Less(tagsCount, 3);
        }
    }
}
