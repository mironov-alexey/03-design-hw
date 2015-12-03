using CommandLine;
using Ninject;
using Nuclex.Game.Packing;
using _03_design_hw.CloudGenerator;
using _03_design_hw.Loaders;
using _03_design_hw.Savers;
using _03_design_hw.Statistics;

namespace _03_design_hw
{
    class Program
    {
        static void Main(string[] args)
        {

            var options = new Options();
            Parser.Default.ParseArguments(args, options);
            var kernel = new StandardKernel();
            kernel.Bind<Options>().ToConstant(options);
            kernel.Bind<ILoader>().To<BaseLoader>().InSingletonScope();
            kernel.Bind<ICloudGenerator>().To<SimpleCloudGenerator>();
            kernel.Bind<ICloudData>().To<CloudData>();
            kernel.Bind<RectanglePacker>()
                .ToConstant(new ArevaloRectanglePacker(int.MaxValue, int.MaxValue))
                .InSingletonScope();
            kernel.Bind<IWordsLoader>().To<WordsLoader>().InSingletonScope();
            kernel.Bind<IBlackListLoader>().To<BlackListLoader>().InSingletonScope();
            var wordsLoader = kernel.Get<WordsLoader>();
            var stat = kernel.Get<StatisticCalculator>().Calculate(wordsLoader.Words);
            kernel.Bind<Statistic>().ToConstant(stat).InSingletonScope();

            var cloudCreator = kernel.Get<ICloudGenerator>();
            using (var cloud = cloudCreator.GenerateCloudImage())
            {
                var cloudSaver = kernel.Get<CloudSaver>();
                cloudSaver.Save(cloud);
            }
            // TODO: как-нибудь вынести метод GetWordLocation в отдельный класс (e.g. Algorithm)
            // TODO: + интерфейс IAlgorithm ? Для поддержки разных алгоритмов расположения прямоугольников на плоскости
        }
    }
}
