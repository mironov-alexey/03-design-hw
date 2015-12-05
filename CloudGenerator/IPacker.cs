using Microsoft.Xna.Framework;

namespace _03_design_hw.CloudGenerator
{
    public interface IPacker
    {
        void TryPack(int width, int height, out Point rectangleLocation);
    }
}