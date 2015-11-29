using System.Drawing;

namespace _03_design_hw.Savers
{
    public interface ICloudSaver
    {
        void Save(Image image);
    }
}