using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    public class NutritionalInformation
    {
        public NutritionalItem? Fat { get; set; }
        public NutritionalItem? SaturatedFat { get; set; }
        public NutritionalItem? Sugar { get; set; }
        public NutritionalItem? Carbohydrates { get; set; }
        public NutritionalItem? Protein { get; set; }
        public NutritionalItem? Fibre { get; set; }
        public NutritionalItem? Salt { get; set; }
        public NutritionalItem? Energy { get; set; }

        public NutritionalInformation()
        {

        }

        public NutritionalInformation(string fat, string saturatedFat, string sugar, string carbohydrates, string protein, string fibre, string salt, string energy)
        {
            Fat = new NutritionalItem(fat);
            SaturatedFat = new NutritionalItem(saturatedFat);
            Sugar = new NutritionalItem(sugar);
            Carbohydrates = new NutritionalItem(carbohydrates);
            Protein = new NutritionalItem(protein);
            Fibre = new NutritionalItem(fibre);
            Salt = new NutritionalItem(salt);
            Energy = new NutritionalItem(energy);
        }
    }
}
