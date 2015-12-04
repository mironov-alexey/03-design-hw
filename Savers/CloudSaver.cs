using System.Drawing;

namespace _03_design_hw.Savers
{
    public class CloudSaver : ICloudSaver
    {
        private readonly string _outputPath;

        public CloudSaver(Options settings)
        {
            _outputPath = settings.OutputPath;
        }

        public void Save(Image image)
        {
            image.Save(_outputPath); // тут можно будет ещё вставить формат, в котором сохраняется картинка
        }
    }
}