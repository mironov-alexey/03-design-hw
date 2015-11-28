using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Xna.Framework;
using Moq;
using NUnit.Framework;
using Color = System.Drawing.Color;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace _03_design_hw.CloudCreator
{
    [TestFixture]
    public class CloudGeneratorShould
    {
        private Mock<ILoader> _loader;
        private Statistic _statistic;
        [SetUp]
        public void SetUp()
        {
            _loader = new Mock<ILoader>();
            _loader.Setup(x => x.BlackList).Returns(new HashSet<string>());
            _loader.Setup(x => x.Top).Returns(3);
            _loader.Setup(x => x.Width).Returns(1024);
            _loader.Setup(x => x.Height).Returns(1024);
            _loader.Setup(x => x.Random).Returns(new Random());
            _loader.Setup(x => x.FontName).Returns("Arial");
            _loader.Setup(x => x.MaxFontSize).Returns(20);
            _loader.Setup(x => x.MinFontSize).Returns(10);
            _loader.Setup(x => x.Colors).Returns(new[] {Color.DarkRed, Color.Black, Color.Coral});
            _loader.Setup(x => x.Words).Returns(new List<string> {"a", "b", "b", "c", "c", "c",});
            _statistic = new Statistic(_loader.Object);
        }
        [Test]
        public void Correctly_GetFont_WithMaxSize()
        {
            var cloudCreator = new Mock<SimpleCloudGenerator>(_loader.Object, _statistic);
            var orderedWords = _statistic.WordsWithFrequency.OrderByDescending(w => w.Frequency).ToList();
            var actualFont = cloudCreator.Object.GetFont(orderedWords[0]);
            Assert.AreEqual(new Font("Arial", 20), actualFont);
        }

        [Test]
        public void Correctly_GetFont_WithMinSize()
        {
            var cloudCreator = new Mock<SimpleCloudGenerator>(_loader.Object, _statistic);
            var orderedWords = _statistic.WordsWithFrequency.OrderByDescending(w => w.Frequency).ToList();
            var actualFont = cloudCreator.Object.GetFont(orderedWords[2]);
            Assert.AreEqual(new Font("Arial", 10), actualFont);
        }

        [Test]
        public void UpdateWidth()
        {
            var cloudCreator = new Mock<SimpleCloudGenerator>(_loader.Object, _statistic);
            cloudCreator.Object.CurrentWidth = cloudCreator.Object.GetNewWidth(new SizeF(10, 10), new XnaPoint(0, 0));
            cloudCreator.Object.CurrentWidth = cloudCreator.Object.GetNewWidth(new SizeF(10, 10), new XnaPoint(10, 0));
            Assert.AreEqual(cloudCreator.Object.CurrentWidth, 20);
        }

        [Test]
        public void UpdateHeight()
        {
            var cloudCreator = new Mock<SimpleCloudGenerator>(_loader.Object, _statistic);
            cloudCreator.Object.CurrentHeight = cloudCreator.Object.GetNewHeight(new SizeF(10, 10), new XnaPoint(0, 0));
            cloudCreator.Object.CurrentHeight = cloudCreator.Object.GetNewHeight(new SizeF(10, 10), new XnaPoint(0, 10));
            Assert.AreEqual(cloudCreator.Object.CurrentHeight, 20);
        }

        [Test]
        public void UpdateSize()
        {
            var cloudCreator = new Mock<SimpleCloudGenerator>(_loader.Object, _statistic);
            var currentSize = new SizeF(cloudCreator.Object.CurrentWidth, cloudCreator.Object.CurrentHeight);
            cloudCreator.Object.GeneratePreReleaseImage();
            var newSize = new SizeF(cloudCreator.Object.CurrentWidth, cloudCreator.Object.CurrentHeight);
            Assert.AreNotEqual(currentSize, newSize);
        }
        [Test]
        public void GetRandomColor()
        {
            var cloudCreator = new Mock<SimpleCloudGenerator>(_loader.Object, _statistic);
            for (var i = 0; i < 1000; i++)
            {
                var color = cloudCreator.Object.GetRandomColor();
                CollectionAssert.Contains(_loader.Object.Colors, color);
            }
        }

        [Test]
        public void Correctly_GenerateImage()
        {
            var cloudCreator = new Mock<SimpleCloudGenerator>(_loader.Object, _statistic);
            using (var img = cloudCreator.Object.GenerateCloudImage())
            {
                Assert.That(img != null);
                Assert.That(img.Size.Height <= _loader.Object.Height && img.Size.Width <= _loader.Object.Width);
            }
        }
    }
}
