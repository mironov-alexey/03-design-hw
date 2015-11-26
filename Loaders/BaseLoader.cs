using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace _03_design_hw
{
    public abstract class BaseLoader
    {
        public string PathToConfig{ get; }

        protected BaseLoader(string pathToConfig)
        {
            PathToConfig = pathToConfig;
            Random = new Random();
        }
        private JObject JsonConfig => JObject.Parse(File.ReadAllText(PathToConfig));
        public virtual IEnumerable<string> Words =>
                File.ReadLines(InputPath)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(x => x.Trim());
        public virtual string InputPath => JsonConfig["words"].ToString();
        private string PathToBlackList => JsonConfig["blacklist"].ToString();
        public virtual HashSet<string> BlackList => 
            new HashSet<string>(
                File.ReadLines(PathToBlackList)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(x => x.Trim()));

        public virtual Color[] Colors => 
            JsonConfig["colors"]
            .Select(item => (string)item)                
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(ColorTranslator.FromHtml)
            .ToArray();

        public virtual string OutputPath => JsonConfig["output"].ToString();

        public Dictionary<string, string> SpellingDictionaries => 
            JsonConfig["dictionaries"]
            .ToDictionary(
            token => token[0].ToString(),
            token => token[1].ToString());

        public virtual string FontName => JsonConfig["fontName"].ToString();
        public virtual int Top => (int) JsonConfig["top"];
        public virtual int MinFontSize => JsonConfig["fontSize"].Select(token => (int)token).ToArray()[0];
        public virtual int MaxFontSize => JsonConfig["fontSize"].Select(token => (int) token).ToArray()[1];
        public virtual int Width => JsonConfig["size"].Select(token => (int)token).ToArray()[0];
        public virtual int Height => JsonConfig["size"].Select(token => (int)token).ToArray()[1];
        public virtual Random Random { get; }
    }
}