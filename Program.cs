using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using CommandLine;
using Ninject;
using Ninject.Parameters;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;
using _03_design_hw.Savers;
using _03_design_hw.Tests;

namespace _03_design_hw
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            Parser.Default.ParseArguments(args, options);
            var kernel = new StandardKernel();
            kernel.Bind<ILoader>().To<DictionaryLoader>().InSingletonScope().WithConstructorArgument(options.ConfigPath);
            kernel.Bind<Statistic.Statistic>().ToSelf().InSingletonScope();
            kernel.Bind<ICloudGenerator>().To<SimpleCloudGenerator>();

            var cloudCreator = kernel.Get<ICloudGenerator>();
            using (var cloud = cloudCreator.GenerateCloudImage())
            {
                var cloudSaver = kernel.Get<CloudSaver>();
                cloudSaver.Save(cloud);
            }
        }
    }
}
