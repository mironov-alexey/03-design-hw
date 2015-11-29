using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace _03_design_hw.Loaders
{
    public abstract class BaseLoader : ILoader
    {
        protected BaseLoader(string pathToConfig)
        {
            PathToConfig = pathToConfig;
            Random = new Random();
        }
        public string PathToConfig { get; }
        public Random Random { get; }
        private JObject JsonConfig => JObject.Parse(File.ReadAllText(PathToConfig));
        public IEnumerable<string> Words =>
                File.ReadLines(InputPath)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(x => x.Trim());
        public string InputPath => JsonConfig["words"].ToString();
        private string PathToBlackList => JsonConfig["blacklist"].ToString();
        public HashSet<string> BlackList => 
            new HashSet<string>(
                File.ReadLines(PathToBlackList)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(x => x.Trim()));

        public Color[] Colors => 
            JsonConfig["colors"]
            .Select(item => (string)item)                
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(ColorTranslator.FromHtml)
            .ToArray();

        public string OutputPath => JsonConfig["output"].ToString();

        public Dictionary<string, string> SpellingDictionaries => 
            JsonConfig["dictionaries"]
            .ToDictionary(
            token => token[0].ToString(),
            token => token[1].ToString());

        public string FontName => JsonConfig["fontName"].ToString();
        public int Top => (int) JsonConfig["top"];
        public int MinFontSize => JsonConfig["fontSize"].Select(token => (int)token).ToArray()[0];
        public int MaxFontSize => JsonConfig["fontSize"].Select(token => (int) token).ToArray()[1];
        public int Width => JsonConfig["size"].Select(token => (int)token).ToArray()[0];
        public int Height => JsonConfig["size"].Select(token => (int)token).ToArray()[1];
    }
}