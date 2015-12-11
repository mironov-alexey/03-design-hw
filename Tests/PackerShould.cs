using Microsoft.Xna.Framework;
using Nuclex.Game.Packing;
using NUnit.Framework;
using _03_design_hw.CloudGenerator;

namespace _03_design_hw.Tests
{
    [TestFixture]
    public class PackerShould
    {
        [SetUp]
        public void SetUp()
        {
            _packer = new ExternalPacker(new ArevaloRectanglePacker(128, 128));
        }

        private ExternalPacker _packer;

        [Test]
        public void BarelyFittingRectangle()
        {
            var placement = _packer.Pack(128, 128);
            Assert.AreEqual(new Point(0, 0), placement);
        }

        [Test]
        public void ReturnNullOnTooLargeRectangle()
        {
            Assert.Throws<OutOfSpaceException>(() => _packer.Pack(129, 129));
        }
    }
}