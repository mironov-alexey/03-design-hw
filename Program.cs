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
//            var wc = new WordCloud.WordCloud(640, 480);
//            var wordCloud = new WordCloud.WordCloud(640, 480);
//            var words = new List<string>
//            {
//                "begin",
//                "end",
//                "start",
//                "work",
//                "works",
//                "apple",
//                "banana",
//            };
//            
//            var freqs = new List<int>
//            {
//                4, 3, 6, 10, 20, 11, 25
//            };

            //            using (Image b = new Bitmap((int)textSize.Width, (int)textSize.Height))
            //            {
            //                using (Graphics g = Graphics.FromImage(b))
            //                {
            //
            //                    Console.WriteLine(textSize);
            //                    g.Clear(Color.Black);
            //                    g.RotateTransform(10);
            //                    g.DrawString("Hello!", new Font("Times New Roman", 20f), new SolidBrush(Color.White), 0, -5);
            //                }
            //                b.Save(@"green.png", ImageFormat.Png);
            //            }
        }

//        private static SizeF GetTextSize(string text)
//        {
//            using (Image tempImage = new Bitmap(1, 1))
//            using (Graphics g = Graphics.FromImage(tempImage))
//                return g.MeasureString(text, new Font("Times New Roman", 20f));
//        }
    }
}
