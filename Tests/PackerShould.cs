using System;
using System.Collections.Generic;
using System.Drawing;
using Moq;
using Nuclex.Game.Packing;
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
        private ExternalPacker _packer;

        [SetUp]
        public void SetUp()
        {
            _packer = new ExternalPacker(new ArevaloRectanglePacker(128, 128));
        }
        
        [Test]
	    public void ReturnNullOnTooLargeRectangle()
        {
            Assert.Throws<OutOfSpaceException>(() => _packer.Pack(129, 129));
        }
	
	    [Test]
	    public void BarelyFittingRectangle()
        {
            Point placement = _packer.Pack(128, 128);
            Assert.AreEqual(new Point(0, 0), placement);
        }
    }
}