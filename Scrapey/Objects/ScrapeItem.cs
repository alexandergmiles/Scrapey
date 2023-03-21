using Microsoft.VisualBasic.FileIO;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    class ScrapeItem
    {
        public string Url { get; set; }
        public int NutrientTableWidth { get; set; }
        public int MineralTableWidth { get; set; }
        public int MineralIndex { get; set; }
        public int NutrientIndex { get; set; }
        public int MineralHundredGramIndex { get; set; }
        public int NutrientHungredGramIndex { get; set; }
        public Func<IPage, IPage> BrowserMethod { get; set; }
        public bool DownloadManually { get; set; }
        public List<Objects.Product>? Products { get; set; }
        public string OutputPath { get; set; }
        public int NutrientTableStartIndex { get; set; }
        public int MineralTableStartIndex { get; set; }
        public int NutrientTableEndIndex { get; set; }
        public int MineralTableEndIndex { get; set; }

        public ScrapeItem(string url, int nutrientTableWidth, int mineralTableWidth, int mineralIndex, int nutrientIndex, int mineralHundredGramIndex, int nutrientHundredGramIndex, Func<IPage, IPage> browserMethod, bool downloadManually, string outputPath, int nutrientTableStartIndex, int mineralTableStartIndex, int nutrientTableEndIndex, int mineralTableEndIndex) =>
            (Url, NutrientTableWidth, MineralTableWidth, MineralIndex, NutrientIndex, MineralHundredGramIndex, NutrientHungredGramIndex, BrowserMethod, DownloadManually, OutputPath, NutrientTableStartIndex, MineralTableStartIndex, NutrientTableEndIndex, MineralTableEndIndex)
            = (url, nutrientTableWidth, mineralTableWidth, mineralIndex, nutrientIndex, mineralHundredGramIndex, nutrientHundredGramIndex, browserMethod, downloadManually, outputPath, nutrientTableStartIndex, mineralTableStartIndex, nutrientTableEndIndex, mineralTableEndIndex);
    }
}
