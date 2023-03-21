using HtmlAgilityPack;
using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    class Scraper
    {
        private HtmlWeb? Loader { get; set; }
        private HtmlDocument? PageBody { get; set; }

        public bool Scrape(string url)
        {
            Loader = new HtmlWeb();
            Loader.UserAgent = "";
            PageBody = Loader.Load(url);

            return true;
        }
        public bool ScrapeFromPage(string content)
        {
            PageBody = new HtmlDocument();
            PageBody.LoadHtml(content);
            return true;
        }
        
        public string GetInnerTextFromXPath(string XPath)
        {
            String scrapedValue = "";
            try
            {
                var nodes = PageBody.DocumentNode.SelectNodes(XPath);
                scrapedValue = nodes.First().InnerText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to read PageBody. Are you sure you've scraped the page?");
            }
            return scrapedValue;
        }
        public string GetInnerTextFromXPath(HtmlNode parentNode, string XPath)
        {
            String scrapedValue = "";
            try
            {
                var body = new HtmlDocument();
                body.LoadHtml(parentNode.InnerHtml);
                scrapedValue = body.DocumentNode.SelectNodes(XPath).First().InnerText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to read PageBody. Are you sure you've scraped the page?");
            }
            return scrapedValue;
        }
        public HtmlNode GetNode(string XPath)
        {
            try
            {
                var result = PageBody.DocumentNode.SelectSingleNode(XPath);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to read PageBody. Are you sure you've scraped the page?");
                Console.WriteLine(ex);
            }
            return null;
        }
        public HtmlNodeCollection GetChildrenNodes(string XPath)
        {
            HtmlNodeCollection childrenNodes = new HtmlNodeCollection(PageBody.DocumentNode);
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
        public HtmlNodeCollection GetChildrenNodes(string XPath, int index)
        {
            HtmlNodeCollection childrenNodes = new HtmlNodeCollection(PageBody.DocumentNode);
            try
            {
                var localNodes = PageBody.DocumentNode.SelectNodes(XPath)[index];
                childrenNodes = localNodes.ChildNodes;
            }
            catch (Exception ex)
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
                var body = new HtmlDocument();
                body.LoadHtml(parentNode.InnerHtml);
                childrenNodes = body.DocumentNode.SelectSingleNode(XPath).ChildNodes;
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

        public List<List<string>> ProcessTable(HtmlNode parentNode, int numberOfColumns)
        {
            if (PageBody == null)
            {
                throw new Exception("You need to load the page body first by scraping");
            }

            var ingredients = new List<string>();

            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
            {
                if (parentNode.ChildNodes[i].ChildNodes.Count > 2)
                {
                    var elements = new List<string>();
                    foreach (var node in parentNode.ChildNodes[i].ChildNodes)
                    {
                        if (node.NodeType == HtmlNodeType.Element && node.InnerText != "nbsp;")
                        {
                            ingredients.Add(Regex.Replace(node.InnerText, @"[^0-9,;():\s+ \w . \n a-zA-Z]+", ""));
                        }
                    }
                }
            }

            var processedIngredients = new List<List<string>>();

            for (int i = 0; i < ingredients.Count - numberOfColumns+1; i += numberOfColumns)
            {
                var localIngredients = new List<string>();
                for (int j = i; j < i + numberOfColumns; j++)
                {
                    localIngredients.Add(Regex.Replace(ingredients[j], @"[^0-9,;():\s+ \n+ \t+ a-zA-Z]+", "").Replace("\n", "").Replace("\t", ""));
                }
                processedIngredients.Add(localIngredients);
            }

            return processedIngredients;
        }

        public List<List<string>> ProcessTable(HtmlNode parentNode, int numberOfColumns, int startIndex)
        {
            if (PageBody == null)
            {
                throw new Exception("You need to load the page body first by scraping");
            }

            var ingredients = new List<string>();

            for (int i = startIndex; i < parentNode.ChildNodes.Count; i++)
            {
                if (parentNode.ChildNodes[i].ChildNodes.Count > numberOfColumns)
                {
                    var divider = false;
                    var count = 0;
                    foreach (var item in parentNode.ChildNodes[i].ChildNodes)
                    {
                        if(item.NodeType == HtmlNodeType.Element)
                        {
                            if(String.IsNullOrEmpty(item.InnerText) || item.InnerText == "&nbsp;")
                            {
                                count++;
                            }
                        }
                    }
                    if(count > 4)
                    {
                        divider = true;
                    }
                    if (divider)
                    {
                        continue;
                    }
                    var elements = new List<string>();
                    foreach (var node in parentNode.ChildNodes[i].ChildNodes)
                    {
                        if (node.NodeType == HtmlNodeType.Element)
                        {
                            ingredients.Add(Regex.Replace(node.InnerText, @"[^0-9,;():\s+ \w . \n a-zA-Z]+", ""));
                        }
                    }
                }
            }

            var processedIngredients = new List<List<string>>();

            for (int i = 0; i < ingredients.Count - numberOfColumns + 1; i += numberOfColumns)
            {
                var localIngredients = new List<string>();
                for (int j = i; j < i + numberOfColumns; j++)
                {
                    localIngredients.Add(ingredients[j].Replace("\n", "").Replace("\t", ""));
                }
                processedIngredients.Add(localIngredients);
            }

            return processedIngredients;
        }

        public List<List<string>> ProcessTable(HtmlNode parentNode, int numberOfColumns, int startIndex, int endIndex)
        {
            if (PageBody == null)
            {
                throw new Exception("You need to load the page body first by scraping");
            }

            var ingredients = new List<string>();

            for (int i = startIndex; i < endIndex; i++)
            {
                if (parentNode.ChildNodes[i].ChildNodes.Count > numberOfColumns)
                {
                    var divider = false;
                    var count = 0;
                    foreach (var item in parentNode.ChildNodes[i].ChildNodes)
                    {
                        if (item.NodeType == HtmlNodeType.Element)
                        {
                            if (String.IsNullOrEmpty(item.InnerText) || item.InnerText == "&nbsp;")
                            {
                                count++;
                            }
                        }
                    }
                    if (count > 4)
                    {
                        divider = true;
                    }
                    if (divider)
                    {
                        continue;
                    }
                    var elements = new List<string>();
                    foreach (var node in parentNode.ChildNodes[i].ChildNodes)
                    {
                        if (node.NodeType == HtmlNodeType.Element)
                        {
                            ingredients.Add(Regex.Replace(node.InnerText, @"[^0-9,;():\s+ \w . \n a-zA-Z]+", ""));
                        }
                    }
                }
            }

            var processedIngredients = new List<List<string>>();

            for (int i = 0; i < ingredients.Count - numberOfColumns + 1; i += numberOfColumns)
            {
                var localIngredients = new List<string>();
                for (int j = i; j < i + numberOfColumns; j++)
                {
                    localIngredients.Add(ingredients[j].Replace("\n", "").Replace("\t", ""));
                }
                processedIngredients.Add(localIngredients);
            }

            return processedIngredients;
        }

        public <List<List<string>> ProcessTableWithManualInputs(HtmlNode parentNode, int numberOfColumns, List<int> skipIndexes)
        {
            if (PageBody == null)
            {
                throw new Exception("You need to load the page body first by scraping");
            }

            var ingredients = new List<string>();

            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
            {
                if (parentNode.ChildNodes[i].ChildNodes.Count > numberOfColumns)
                {
                    var elements = new List<string>();
                    foreach (var node in parentNode.ChildNodes[i].ChildNodes)
                    {
                        if (node.NodeType == HtmlNodeType.Element)
                        {
                            ingredients.Add(Regex.Replace(node.InnerText, @"[^0-9,;():\s+ \w . \n a-zA-Z]+", ""));
                        }
                    }
                }
            }
        }

        public int FindIndex(HtmlNode parentNode, int searchColumn, string ValueToFind)
        {
            for(int i = 0; i < parentNode.ChildNodes.Count; i++)
            {
                if (parentNode.ChildNodes[i].InnerHtml.Contains(ValueToFind))
                {
                    return i;
                }
            }
            return -1;
        }

        public Dictionary<string, string> ProcessNutrients(List<String> input)
        {
            var nutrientToFind = new List<List<string>> {
                new List<string>
                {
                    "Fat"
                },
                new List<string>
                {
                    "Saturated Fat", "Of which saturates"
                },
                new List<string>
                {
                    "Sugar", "Sugars", "of which sugars"
                },
                new List<string>
                {
                    "Carbohydrates", "Carbohydrate"
                },
                new List<string>
                {
                    "Protein"
                },
                new List<string>()
                {
                    "Fibre"
                },
                new List<string>()
                {
                    "Salt"
                },
                new List<string>()
                {
                    "Calories", "Energy", "kcals"
                }
            };

            Dictionary<string, string> inputAsDictionary = new Dictionary<string, string>();

            foreach (var item in input)
            {
                foreach (var nutrient in nutrientToFind)
                {
                    for(int i = 0; i < nutrient.Count; i++)
                    {
                        if (item.ToLower().Contains(nutrient[i].ToLower()))
                        {
                            inputAsDictionary[nutrient[0]] = item;
                            Console.WriteLine($"{nutrient[0]} {item} ");
                            break;
                        }
                    }
                }
            }

             return inputAsDictionary;
        }

        public List<NutritionalItem> GetNutrients(List<String> input)
        {
            var nutrientInformation = new List<NutritionalItem>();
            foreach (var item in input)
            {
                nutrientInformation.Add(new NutritionalItem(item));
            }
            return nutrientInformation;
        }

        public List<NutritionalItem> GetMinerals(List<String> input)
        {
            var mineralInformation = new List<NutritionalItem>();
            foreach(var item in input)
            {
                mineralInformation.Add(new NutritionalItem(item));
            }    
            return mineralInformation;
        }
    }
}