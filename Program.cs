using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using CommandLine;
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
            Options options = new Options();
            Parser.Default.ParseArguments(args, options);
            var kernel = new StandardKernel();
            kernel.Bind<BaseLoader>().To<DictionaryLoader>().WithConstructorArgument(options.ConfigPath);
            kernel.Bind<ICloudCreator>().To<SimpleCloudCreator>();
            kernel.Get<ICloudCreator>().GeneratePreReleaseImage();
        }
    }
}
