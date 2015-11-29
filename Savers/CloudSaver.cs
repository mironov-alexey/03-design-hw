using System.Drawing;
using _03_design_hw.Loaders;

namespace _03_design_hw.Savers
{
    public class CloudSaver : ICloudSaver
    {
        private ILoader Loader { get; }

        public CloudSaver(ILoader loader)
        {
            Loader = loader;
        }

        public void Save(Image image)
        {
            image.Save(Loader.OutputPath); // тут можно будет ещё вставить формат, в котором сохраняется картинка
        }
    }
}
