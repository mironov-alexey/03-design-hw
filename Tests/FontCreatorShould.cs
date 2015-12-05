using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;

namespace _03_design_hw.Tests
{
    [TestFixture]
    internal class FontCreatorShould
    {
        [SetUp]
        public void SetUp()
        {
            _settings = new Settings {MaxFontSize = 20, MinFontSize = 10, FontName = "Arial"};
            _statistic = new Statistic(new List<Word>
            {
                new Word("a", 10),
                new Word("b", 3),
                new Word("c", 1)
            });
            _fontCreator = new FontCreator(_settings);
        }

        private IFontCreator _fontCreator;
        private Settings _settings;
        private Statistic _statistic;

        [Test]
        public void Correctly_GetFontMaxSize()
        {
            var font = _fontCreator.GetFont(_statistic, new Word("a", 10));
            var expectedFont = new Font("Arial", 20);
            Assert.AreEqual(expectedFont, font);
        }

        [Test]
        public void Correctly_GetFontMiddleSize()
        {
            var font = _fontCreator.GetFont(_statistic, new Word("b", 3));
            var expectedFont = new Font("Arial", 14);
            Assert.AreEqual(expectedFont, font);
        }

        [Test]
        public void Correctly_GetFontMinSize()
        {
            var font = _fontCreator.GetFont(_statistic, new Word("c", 1));
            var expectedFont = new Font("Arial", 10);
            Assert.AreEqual(expectedFont, font);
        }
    }
}