using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _03_design_hw
{
    public class TagCloudSettings
    {
        public virtual string FontName => Loader.FontName;
        public virtual int Top => Loader.Top;
        public virtual int MinFontSize => Loader.MinFontSize;
        public virtual int MaxFontSize => Loader.MaxFontSize;
        public virtual string InputPath => Loader.InputPath;
        public virtual string OutputPath => Loader.OutputPath;
        public virtual Color[] Colors => Loader.Colors;
        public virtual HashSet<string> BlackList => Loader.BlackList;
        public virtual Dictionary<string, string> SpellingDictionaries => Loader.SpellingDictionaries;
        private BaseLoader Loader{ get; }
        public virtual Random Random { get; }


        public TagCloudSettings(BaseLoader loader)
        {
            Loader = loader;
            Random = new Random();
        }
    }
}