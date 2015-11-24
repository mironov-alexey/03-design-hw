using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using NHunspell;
using Ninject;
using WordCloud;
using Mapper;

namespace _03_design_hw
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new[] {"config.json"};
            var kernel = new StandardKernel();
            kernel.Bind<BaseLoader>().To<DictionaryLoader>().WithConstructorArgument(args[0]);
            kernel.Bind<ICloudCreator>().To<SimpleCloudCreator>();
            kernel.Get<ICloudCreator>().DrawAndSaveCloudImage();
        }
    }
}
