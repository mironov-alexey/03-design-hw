using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using CommandLine;
using Ninject;
using Ninject.Parameters;

namespace _03_design_hw
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            Parser.Default.ParseArguments(args, options);
            var kernel = new StandardKernel();
            kernel.Bind<BaseLoader>().To<DictionaryLoader>().InSingletonScope().WithConstructorArgument(options.ConfigPath);
            kernel.Bind<Statistic>().ToSelf().InSingletonScope().WithConstructorArgument("settings", "words");
            kernel.Bind<ICloudCreator>().To<SimpleCloudCreator>();

            var statistic = kernel.Get<Statistic>(
                    new ConstructorArgument("settings", kernel.Get<BaseLoader>())
                );
            var cloudCreator = kernel.Get<ICloudCreator>();
            var cloud = cloudCreator.GenerateReleaseImage(statistic.WordsWithFrequency);
            var cloudSaver = kernel.Get<CloudSaver>();
            cloudSaver.Save(cloud);;
        }
    }
}
