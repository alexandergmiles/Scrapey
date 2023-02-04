using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapey.Interfaces
{
    internal interface IScraper
    {
        /// <summary>
        /// Do the actual scraping
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool Scrape(string url);
        /// <summary>
        /// Get the content of the XPath
        /// </summary>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public string GetInnerTextFromXPath(string XPath);

        public string GetPrice();

        public string GetName();

        public string GetIngredients();
        public string GetIngredients(string textToStrip);

        public string GetOverview();

        public string GetScore();

        public List<string> GetAllProductsToScrape();


        public Dictionary<string, string> DynamicallyGetXPathsBasedOnClass(string parentXPath, Dictionary<string, string> Input);
    }
}
