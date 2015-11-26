using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_design_hw
{
    public class CloudSaver
    {
        private TagCloudSettings Settings{ get; }

        public CloudSaver(TagCloudSettings settings)
        {
            Settings = settings;
        }

        public void Save(Image image)
        {
            image.Save(Settings.OutputPath); // тут можно будет ещё вставить формат, в котором сохраняется картинка
        }
    }
}
