using Microsoft.Xna.Framework;

namespace _03_design_hw.CloudGenerator
{
    public interface IPacker
    {
        Point Pack(int width, int height);
    }
}