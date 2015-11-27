using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_design_hw
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
