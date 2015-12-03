using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace _03_design_hw.Loaders
{
    public class BaseLoader : ILoader
    {
        public BaseLoader(Options options)
        {
            _pathToConfig = options.ConfigPath;
            Random = new Random();
        }

        private readonly string _pathToConfig;

        public Random Random { get; }

        private JObject JsonConfig => JObject.Parse(File.ReadAllText(_pathToConfig));

        public Color[] Colors => 
            JsonConfig["colors"]
            .Select(item => (string)item)                
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(ColorTranslator.FromHtml)
            .ToArray();

        public Dictionary<string, string> SpellingDictionaries => 
            JsonConfig["dictionaries"]
            .ToDictionary(
            token => token[0].ToString(),
            token => token[1].ToString());

        public string FontName => JsonConfig["fontName"].ToString();

        public int TagsCount => (int) JsonConfig["tagsCount"];

        public int MinFontSize => JsonConfig["fontSize"].Select(token => (int)token).ToArray()[0];

        public int MaxFontSize => JsonConfig["fontSize"].Select(token => (int) token).ToArray()[1];

        public int Width => JsonConfig["size"].Select(token => (int)token).ToArray()[0];

        public int Height => JsonConfig["size"].Select(token => (int)token).ToArray()[1];
    }
}