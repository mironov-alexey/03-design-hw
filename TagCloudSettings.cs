using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace _03_design_hw
{
    public class TagCloudSettings
    {
        public Size Size{ get; }
        public string FontName{ get; }
        public int Top{ get; }
        public int MinFontSize{ get; }
        public int MaxFontSize{ get; }
        public Color[] Colors{ get; }
        public HashSet<string> BlackList{ get; } 
        public TagCloudSettings(Size size, Color[] colors, HashSet<string> blackList, string fontName="Times New Roman", int top=0, int minFontSize=12, int maxFontSize=24)
        {
            Size = size;
            Colors = colors;
            BlackList = blackList;
            FontName = fontName;
            Top = top;
            MinFontSize = minFontSize;
            MaxFontSize = maxFontSize;
        }
        
    }
}