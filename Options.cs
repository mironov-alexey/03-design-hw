using System.Collections.Generic;
using CommandLine;

namespace _03_design_hw
{
    public class Options
    {
        [Option('c', "config", DefaultValue = "config.json",
            HelpText = "Path to config file.")]
        public string ConfigPath{ get; set; }

        [Option('w', "words", DefaultValue = "words.txt",
            HelpText = "Path to words file.")]
        public string PathToWords { get; set; }

        [Option('b', "blacklist", DefaultValue = "blacklist.txt",
            HelpText = "Path to blacklist file.")]
        public string PathToBlackList { get; set; }

        [Option('o', "output", DefaultValue = "cloud.png",
            HelpText = "Path to output file with tags cloud image.")]
        public string OutputPath{ get; set; }
    }
}