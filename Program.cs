using CommandLine;
using Ninject;
using Nuclex.Game.Packing;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;
using _03_design_hw.Savers;
using _03_design_hw.Statistics;

namespace _03_design_hw
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            Parser.Default.ParseArguments(args, options);
            StandardKernel kernel = GetKernel(options);
            var cloudCreator = kernel.Get<ICloudGenerator>();
            using (var cloud = cloudCreator.GenerateCloudImage())
            {
                var cloudSaver = kernel.Get<CloudSaver>();
                cloudSaver.Save(cloud);
            }
        }

        private static StandardKernel GetKernel(Options options)
        {
            var kernel = new StandardKernel();
            BindInterfaces(options, kernel);
            BindSettings(kernel);
            BindStatistic(kernel);
            return kernel;
        }

        private static void BindStatistic(StandardKernel kernel)
        {
            var wordsLoader = kernel.Get<WordsLoader>();
            var stat = kernel.Get<StatisticCalculator>().Calculate(wordsLoader.Words);
            kernel.Bind<Statistic>().ToConstant(stat).InSingletonScope();
        }

        private static void BindInterfaces(Options options, StandardKernel kernel)
        {
            kernel.Bind<Options>().ToConstant(options);
            kernel.Bind<ICloudGenerator>().To<SimpleCloudGenerator>();
            kernel.Bind<ICloudData>().To<CloudData>();
            kernel.Bind<RectanglePacker>()
                .ToConstant(new ArevaloRectanglePacker(int.MaxValue, int.MaxValue))
                .InSingletonScope();

            kernel.Bind<ISettingsLoader>().To<BaseSettingsLoader>().InSingletonScope();
            kernel.Bind<IWordsLoader>().To<WordsLoader>().InSingletonScope();
            kernel.Bind<IBlackListLoader>().To<BlackListLoader>().InSingletonScope();
            kernel.Bind<IFontCreator>().To<FontCreator>();
        }

        private static void BindSettings(StandardKernel kernel)
        {
            var settingsLoader = kernel.Get<ISettingsLoader>();
            kernel.Bind<Settings>().ToConstant(settingsLoader.Load());
        }
    }
}