using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    class ProductDTO
    {
        public string ID { get; set; }
        public string URL { get; set; }
        public string NameXPath { get; set; }
        public string PriceXPath { get; set; }
        public string ScoreXPath { get; set; }
        public string NutrientInformationXPath { get; set; }
        public string MineralsInformationXPath { get; set; }
        public string IngredientsXPath { get; set; }
        public List<string> PicturesXPath { get; set; }
        public int PageType { get; set; }
        public int NutrientTableIndex { get; set; }
        public int NutrientTableWidth { get; set; }
        public int MineralTableIndex { get; set; }
        public int MineralTableWidth { get; set; }
        public string MineralOneHundredGramsColumn { get; set; }
        public string NutrientOneHundredGramsColumn { get; set; }
        public ProductDTO(string iD,
            string uRL,
            string nameXPath,
            string priceXPath,
            string scoreXPath,
            string nutrientInformationXPath,
            string mineralsInformationXPath,
            string ingredientsXPath, 
            List<string> picturesXPath,
            int pageType,
            int mineralTableIndex,
            int mineralTableWidth,
            int nutrientTableIndex,
            int nutrientTableWidth,
            string mineralOneHundredGramsColumn,
            string nutrientOneHundredGramsColumn)
        {
            ID = iD;
            URL = uRL;
            NameXPath = nameXPath;
            PriceXPath = priceXPath;
            ScoreXPath = scoreXPath;
            NutrientInformationXPath = nutrientInformationXPath;
            MineralsInformationXPath = mineralsInformationXPath;
            IngredientsXPath = ingredientsXPath;
            PicturesXPath = picturesXPath;
            PageType = pageType;
            MineralTableIndex = mineralTableIndex;
            MineralTableWidth = mineralTableWidth;
            NutrientTableIndex = nutrientTableIndex;
            NutrientTableWidth = nutrientTableWidth;
            MineralOneHundredGramsColumn = mineralOneHundredGramsColumn;
            NutrientOneHundredGramsColumn = nutrientOneHundredGramsColumn;  
        }
    }
}
