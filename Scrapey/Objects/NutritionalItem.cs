using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    public struct NutritionalItem
    { 
        public String Nutrient { get; set; }
        public String Unit { get; set; }
        public String Quantity { get; set; }
        public NutritionalItem()
        {

        }

        public NutritionalItem(string nutrient, string unit, string quantity)
        {
            Nutrient = nutrient;
            Unit = unit;
            Quantity = quantity;
        }
        public NutritionalItem(string input)
        {
            var units = new Dictionary<string, string>
            {
                {  "(g)", "g"},
                {  "(kcal)", "kcal" },
                {  "(µg)", "µg" },
                {  "µg", "µg" },
                {  "(mg)", "mg" },
                { "(μg RE)", "μg RE" },
                { "(mg TE)", " mg TE" },
                { "(mg NE)", "mg NE" },
            };
            var split = input.Split(":");
            foreach(var key in units.Keys)
            {
                var found = false;
                if (split.Length > 1)
                {
                    if (split[0].Contains(key))
                    {
                        Unit = units[key];
                        Nutrient = split[0].Replace(key, "").Replace("(", "");
                        Quantity = split[1].Replace(")", "");
                        found = true;
                        break;
                    }
                }
                if(found == false && split.Length > 1)
                {   
                    Unit = "";
                    Nutrient = split[0].Replace(key, "").Replace("(", "");
                    Quantity = split[1].Replace(")", "");
                }
            }
            //if (split[0].Contains("(g)"))
            //{
            //    Unit = "g";
            //    Nutrient = split[0].Replace("(g)", "").Replace("(", "");
            //    Quantity = split[1].Replace(")", "");
            //}
            //if (split[0].Contains("(kcal)"))
            //{
            //    Unit = "kcal";
            //    Nutrient = split[0].Replace("(kcal)", "").Replace("(", "");
            //    Quantity = split[1].Replace(")", "");
            //}
            //if (split[0].Contains("(µg)"))
            //{
            //    Unit = "µg";
            //    Nutrient = split[0].Replace("(µg)", "").Replace("(", "");
            //    Quantity = split[1].Replace(")", "");
            //}
            //if (split[0].Contains("(µg)"))
            //{
            //    Unit = "µg";
            //    Nutrient = split[0].Replace("(µg)", "").Replace("(", "");
            //    Quantity = split[1].Replace(")", "");
            //}
        }
    }
}
