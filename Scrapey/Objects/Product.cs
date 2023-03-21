using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    public class Product
    {
        public Product(string name, string price, string score, string nutInfo, string minInfo, string ingredients, List<String> pics) =>
            (Name, Price, Score, NutrientInformation, MineralInformation, Ingredients, Pictures) = (name, price, score, nutInfo, minInfo, ingredients, pics);

        public String Name { get; set; }
        public String Price { get; set; }
        public String Score { get; set; }
        public String NutrientInformation { get; set; }
        public string MineralInformation { get; set; }
        public String Ingredients { get; set; }
        public List<String> Pictures { get; set;  }

        public override string ToString()
        {
            return $"{Name} {Price} {Score} \n {NutrientInformation} \n {MineralInformation}";
        }
    }
}
