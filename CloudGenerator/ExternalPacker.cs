using Microsoft.Xna.Framework;
using Nuclex.Game.Packing;

namespace _03_design_hw.CloudGenerator
{
    public class ExternalPacker : IPacker
    {
        private readonly RectanglePacker _packer;

        public ExternalPacker(RectanglePacker packer)
        {
            _packer = packer;
        }

        public void TryPack(int width, int height, out Point rectangleLocation) =>
            _packer.TryPack(width, height, out rectangleLocation);
    }
}
