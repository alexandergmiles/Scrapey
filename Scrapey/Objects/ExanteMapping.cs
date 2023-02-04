using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    class ExanteMapping
    {
        public string Overview { get; set; }
        public string Ingredients { get; set; }
        public string Price { get; set; }

        public ExanteMapping()
        {
            Overview = string.Empty;
            Ingredients = string.Empty;
        }

        public ExanteMapping(string overview, string ingredients, string price) => (Overview, Ingredients, Price) = (overview, ingredients, price);
    }
}
