using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _03_design_hw
{
    public class TagCloudSettings
    {
        public string FontName{ get; }
        public int Top{ get; }
        public int MinFontSize{ get; }
        public int MaxFontSize{ get; }
        public string InputPath{ get; set; }
        public string OutputPath{ get; set; }
        public Color[] Colors{ get; }
        public HashSet<string> BlackList{ get; }
        public Random Random{ get; }

        public Dictionary<string, string> SpellingDictionaries { get; }

        public TagCloudSettings(BaseLoader loader)
        {
            InputPath = loader.InputPath;
            OutputPath = loader.OutputPath;
            Colors = loader.Colors;
            BlackList = loader.BlackList;
            FontName = loader.FontName;
            Top = loader.Top;
            MinFontSize = loader.MinFontSize;
            MaxFontSize = loader.MaxFontSize;
            SpellingDictionaries = loader.SpellingDictionaries;
            Random = new Random();
        }
    }
}