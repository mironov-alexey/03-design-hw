using System;
using System.Collections.Generic;
using System.Drawing;
using Moq;
using NUnit.Framework;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;
using _03_design_hw.Statistics;
using Point = Microsoft.Xna.Framework.Point;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class PackerShould
    {
        [SetUp]
        public void SetUp()
        {
            _packer = new Mock<IPacker>();
            _settings = new Settings
            {
                Colors = new[] {Color.Black},
                FontName = "Arial",
                MaxFontSize = 20,
                MinFontSize = 10,
                TagsCount = 10
            };
            _fontCreator = new FontCreator(_settings);
            _cloudData = new CloudData(_settings, _packer.Object, _fontCreator);
            _stat = new Statistic(new List<Word>
            {
                new Word("a", 10),
                new Word("b", 1)
            });
        }

        private Mock<IPacker> _packer;
        private IFontCreator _fontCreator;
        private Settings _settings;
        private CloudData _cloudData;
        private Statistic _stat;

        [Test]
        public void Correctly_GetLocation()
        {
            var rand = new Random();
            foreach (var tag in _cloudData.GetTags(_stat))
            {
                var p = new Point(rand.Next(10, 100), rand.Next(10, 100));
                _packer.Setup(x => x.Pack(10, 10)).Returns(p);
                Assert.AreEqual(tag.Location, p);
            }
        }
    }
}