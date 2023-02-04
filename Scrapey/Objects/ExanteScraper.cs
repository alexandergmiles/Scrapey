using HtmlAgilityPack;
using Scrapey.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    internal class ExanteScraper : IScraper
    {
        private HtmlWeb? Loader { get; set; }
        private HtmlDocument? PageBody { get; set; }

        public ExanteMapping Mapping { get; set; }


        public bool Scrape(string url)
        {
            Loader = new HtmlWeb();
            Loader.UserAgent = "";
            PageBody = Loader.Load(url);
            
            return true;
        }

        public string GetInnerTextFromXPath(string XPath)
        {
            String scrapedValue = "";
            try
            {
                scrapedValue = PageBody.DocumentNode.SelectNodes(XPath).First().InnerText;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to read PageBody. Are you sure you've scraped the page?");
            }
            return scrapedValue;
        }

        public HtmlNodeCollection GetChildrenNodes(string XPath)
        {
            HtmlNodeCollection childrenNodes = new HtmlNodeCollection(PageBody.DocumentNode);
            try
            {
                childrenNodes = PageBody.DocumentNode.SelectSingleNode(XPath).ChildNodes;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to get children nodes from given XPath");
            }

            return childrenNodes;
        }

        public HtmlNodeCollection GetChildrenNodes(HtmlNode parentNode, string XPath)
        {
            HtmlNodeCollection childrenNodes = new HtmlNodeCollection(parentNode);
            try
            {
                childrenNodes = PageBody.DocumentNode.SelectSingleNode(XPath).ChildNodes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get children nodes from given XPath");
            }

            return childrenNodes;
        }

        public HtmlNodeCollection GetAllInstances(string XPath)
        {
            return PageBody.DocumentNode.SelectNodes(XPath);
        }

        public string GetAttributeValue(string XPath, string attribute)
        {
            String scrapedValue = "";
            try
            {
                var node = PageBody.DocumentNode.SelectSingleNode(XPath);
                scrapedValue = node.Attributes[attribute].Value;
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Unable to read attribute.");
            }

            return scrapedValue;
        }

        public string GetPrice()
        {
            if(PageBody == null)
            {
                throw new PageNotScrapedException();
            }

            //var rawPrice = GetInnerTextFromXPath("/html/body/div[3]/div/main/div[3]/div[2]/div/div[1]/div[6]/div/div/span/p");
            var rawPrice = GetInnerTextFromXPath("//p[contains(@class, 'productPrice_price  ')]");
            rawPrice = rawPrice.Replace("&#163;", "");

            return rawPrice;
        }

        public string GetName()
        {
            if(PageBody == null)
            {
                throw new Exception("You need to load the page body first by scraping");
            }

            var rawName = GetInnerTextFromXPath("/html/body/div[3]/div/main/div[3]/div[2]/div/div[1]/div[2]/div/h1");
            return rawName;
        }

        public string GetIngredients()
        {
            if(PageBody == null)
            {
                throw new PageNotScrapedException();
            }
            var nodes = GetChildrenNodes(Mapping.Ingredients);
            //var nodes = GetChildrenNodes("/html/body/div[3]/div/main/div[3]/div[1]/div[2]/div[2]/div/div/div/div[4]/div/div/div");

            var builder = new StringBuilder();

            foreach (var node in nodes)
            {
                builder.Append(node.InnerText.Trim());
                if (node.NodeType == HtmlNodeType.Element)
                {
                    //Console.WriteLine($"ALLERGEN: {node.InnerText.Trim()}");
                }
            }

            var ingredients = builder.ToString().Replace("ALLERGENS: For allergens, see ingredients inbold. May contain peanuts, egg, milk and gluten.", "");
            return ingredients;
        }
        public string GetIngredients(string textToStrip)
        {
            if (PageBody == null)
            {
                throw new PageNotScrapedException();
            }
            var nodes = GetChildrenNodes(Mapping.Ingredients);

            var builder = new StringBuilder();

            foreach (var node in nodes)
            {
                builder.Append(node.InnerText.Trim());
            }

            var ingredients = builder.ToString().Replace(textToStrip, "");
            return ingredients;
        }

        public string GetOverview()
        {
            if (PageBody == null)
            {
                throw new PageNotScrapedException();
            }

            var rawOverview = GetInnerTextFromXPath(Mapping.Overview);
            return rawOverview;
        }

        public string GetScore()
        {
            if (PageBody == null) { 
                throw new PageNotScrapedException();
            }

            var rawScore = GetInnerTextFromXPath("/html/body/div[3]/div/main/div[5]/div/div/div[2]/div[2]/div[1]/div/div[1]/div/div/span");
            rawScore = rawScore.Replace(" Stars", "");
            return rawScore;
        }

        public List<string> GetAllProductsToScrape()
        {
            var productLinks = new List<string>();
            if (PageBody == null)
            {
                throw new PageNotScrapedException();
            }

            var products = GetAllInstances("//a[contains(@class, 'productBlock_link')]");

            foreach(var item in products)
            {
                productLinks.Add(item.Attributes["href"].Value);
            }

            return productLinks;
        }

        public Dictionary<string, string> DynamicallyGetXPathsBasedOnClass(string parentXPath, Dictionary<string, string> Input)
        {
            var XPaths = new Dictionary<string, string>();
            var childrenNodes = GetChildrenNodes(parentXPath);

            foreach(var item in childrenNodes)
            {
               foreach(var key in Input)
                    if (item.HasAttributes)
                    {
                        foreach(var attribute in item.Attributes)
                        {
                            if (attribute.Value.Contains(key.Value))
                            {
                                XPaths[key.Key] = (item.XPath);
                            }
                        }
                    }
                }

            return XPaths;
        }
    }
}
