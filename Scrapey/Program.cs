using Scrapey.Interfaces;
using Scrapey.Objects;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using HtmlAgilityPack;
using System.Text;
using PuppeteerSharp;
using static System.Formats.Asn1.AsnWriter;
using CsvHelper;
using System.Globalization;
using System.Net.WebSockets;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.VisualBasic;
using CsvHelper.Configuration;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Primitives;
using FastExcel;

namespace Scrapey
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configurationPath = @"Configuration/config.json";
            var extensions = new VendorExtensions();
            var results = new List<List<Objects.Product>>();
            var scraper = new Scraper();
            Configuration config = new Configuration();

            using(var reader = new StreamReader(configurationPath))
            { 
                config = JsonConvert.DeserializeObject<Configuration>(reader.ReadToEnd());
            }
            var paths = new Dictionary<String, ConfigurationPath>();

            foreach(var configItem in config.ProductFiles)
            {
                paths[configItem.Name] = configItem;
            }

            Func<IPage, IPage> ShakeThatWeightBrowserMethod = (page) =>
            {
                System.Threading.Thread.Sleep(2000);
                page.EvaluateExpressionAsync("document.querySelector(\"#accordion-title-reviews > button\").click()");
                return page;
            };

            Func<IPage, IPage> SlimFastBrowserMethod = (page) =>
            {
                return page;
            };
            Func<IPage, IPage> SlimSaveBrowserMethod = (page) =>
            {
                return page;
            };
            Func<IPage, IPage> OptiFastBrowserMethod = (page) =>
            {
                return page;
            };
            Func<IPage, IPage> LighterLifeBrowserMethod = (page) =>
            {
                return page;
            };
            Func<IPage, IPage> ExanteBrowserMethod = (page) =>
            {
                return page;
            };
            
            var toBeScraped = new ScrapeItem[]
            {
                //new ScrapeItem(paths["ShakeThatWeight"].FilePath, 3, 5, 5, 5, 1, 1, ShakeThatWeightBrowserMethod, true, paths["ShakeThatWeight"].OutputFile, 0, 0, -1, -1),
                //new ScrapeItem(paths["SlimFast"].FilePath, 3, 4, 1, 1, 1, 1, SlimFastBrowserMethod, true, paths["SlimFast"].OutputFile, 0, 5, -1, -1),
                //new ScrapeItem(paths["SlimSave"].FilePath, 4, 4, 1, 1, 1, 1, SlimSaveBrowserMethod, true, paths["SlimSave"].OutputFile, 3, 3, -1, -1),
                new ScrapeItem(paths["OptiFast"].FilePath, 7, 7, 3, 3, 3, 3, OptiFastBrowserMethod, true, paths["OptiFast"].OutputFile, 0, 29, 28, -1),
                //new scrapeitem(paths["lighterlife"].filepath, 5, 5, 2, 2, 1, 1, lighterlifebrowsermethod, true, paths["lighterlife"].outputfile, 0, 0, -1, -1),
                //new scrapeitem(paths["exante"].filepath, 3, 3, 3, 3, 3, 3, exantebrowsermethod, true, paths["exante"].outputfile, 0, 0, -1, -1)
            };

            foreach (var item in toBeScraped)
            { 
                var browser = Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe", DefaultViewport = new ViewPortOptions { Height = 1080, Width = 1920 } }).Result;
                item.Products = await ScrapeSiteForProducts(item, scraper, browser);
                foreach (var product in item.Products)
                {
                    var nutrients = product.NutrientInformation.Split(",");
                    var nutinfo = scraper.GetNutrients(product.NutrientInformation.Split(",").ToList<String>());
                    foreach(var nutin in nutinfo)
                    {
                        Console.WriteLine($"{nutin.Nutrient} {nutin.Unit} {nutin.Quantity} ");
                    }
                    Console.WriteLine();
                    var mininfo = scraper.GetMinerals(product.MineralInformation.Split(",").ToList<String>());
                }
                //WriteCSV(item.Products, item.OutputPath);
            }
        }
        
        public static void PrintProducts(List<Objects.Product> products)
        {
            foreach(var product in products)
            {
                Console.WriteLine(product.ToString());
            }
        }

        public static async Task<List<Objects.Product>> ScrapeSiteForProducts(ScrapeItem item, Scraper scraper, IBrowser browser)
        {
            var returnProducts = new List<Objects.Product>();
            try
            {
                var products = new List<ProductDTO>();

                using (StreamReader file = File.OpenText(item.Url))
                {
                    products = JsonConvert.DeserializeObject<List<ProductDTO>>(file.ReadToEnd());
                }
                
                foreach (var product in products)
                {
                    Console.WriteLine($"Downloading {product.URL} page content...");
                    var content = "";
                    if (item.DownloadManually)
                    {
                        var page = await browser.NewPageAsync();
                        await page.GoToAsync(product.URL);

                        page = item.BrowserMethod.Invoke(page);

                        content = await page.GetContentAsync();
                        scraper.ScrapeFromPage(content);
                    }
                    else
                    {
                        scraper.Scrape(product.URL);
                    }

                    var minerals = new List<List<string>>();
                    var nutrients = new List<List<string>>();

                    var nameXpathLocal = (String.IsNullOrEmpty(product.NameXPath)) ? products[0].NameXPath : product.NameXPath;
                    var priceXpathLocal = (String.IsNullOrEmpty(product.PriceXPath)) ? products[0].PriceXPath : product.PriceXPath;
                    var scoreXpathLocal = (String.IsNullOrEmpty(product.ScoreXPath)) ? products[0].ScoreXPath : product.ScoreXPath;
                    var ingredientsXpathLocal = (String.IsNullOrEmpty(product.IngredientsXPath)) ? products[0].IngredientsXPath : product.IngredientsXPath;
                    var nutritionalInformationXpathLocal = (String.IsNullOrEmpty(product.NutrientInformationXPath)) ? products[0].NutrientInformationXPath : product.NutrientInformationXPath;
                    var mineralInformationXpathLocal = (String.IsNullOrEmpty(product.MineralsInformationXPath)) ? products[0].MineralsInformationXPath : product.MineralsInformationXPath;
                    var mineralOneHundredGramsColumn = 0;
                    var nutrientOneHundredGramsColumn = 0;

                    if (String.IsNullOrEmpty(product.MineralOneHundredGramsColumn))
                    {
                        mineralOneHundredGramsColumn = item.MineralHundredGramIndex;
                    }
                    else
                    {
                        mineralOneHundredGramsColumn = Convert.ToInt32(product.MineralOneHundredGramsColumn);
                    }
                    if (String.IsNullOrEmpty(product.NutrientOneHundredGramsColumn))
                    {
                        nutrientOneHundredGramsColumn = item.NutrientHungredGramIndex;
                    }
                    else
                    {
                        nutrientOneHundredGramsColumn = Convert.ToInt32(product.NutrientOneHundredGramsColumn);
                    }
                    var name =  scraper.GetInnerTextFromXPath(nameXpathLocal);
                    var price = scraper.GetInnerTextFromXPath(priceXpathLocal);
                    var score = scraper.GetInnerTextFromXPath(scoreXpathLocal);
                    var ingredients = scraper.GetInnerTextFromXPath(ingredientsXpathLocal);

                    if (product.MineralTableIndex == 0)
                    {
                        product.NutrientTableIndex = item.NutrientIndex;
                        product.NutrientTableWidth = item.NutrientTableWidth;
                        product.MineralTableIndex = item.MineralIndex;
                        product.MineralTableWidth = item.MineralTableWidth;
                    }

                    if(!String.IsNullOrEmpty(nutritionalInformationXpathLocal) && String.IsNullOrEmpty(mineralInformationXpathLocal))
                    {
                        int indexToGetTo = scraper.FindIndex(scraper.GetChildrenNodes(nutritionalInformationXpathLocal)[product.NutrientTableIndex], 0, "Vitamins");
                        nutrients = scraper.ProcessTable(scraper.GetChildrenNodes(nutritionalInformationXpathLocal)[product.NutrientTableIndex], product.NutrientTableWidth, item.NutrientTableStartIndex, indexToGetTo-1);
                        minerals = scraper.ProcessTable(scraper.GetChildrenNodes(nutritionalInformationXpathLocal)[product.NutrientTableIndex], product.NutrientTableWidth, indexToGetTo+1);
                    }

                    if (!String.IsNullOrEmpty(nutritionalInformationXpathLocal))
                        nutrients = scraper.ProcessTable(scraper.GetChildrenNodes(nutritionalInformationXpathLocal)[product.NutrientTableIndex], product.NutrientTableWidth, item.NutrientTableStartIndex, 32);

                    if (!String.IsNullOrEmpty(mineralInformationXpathLocal))
                        minerals = scraper.ProcessTable(scraper.GetChildrenNodes(mineralInformationXpathLocal)[product.MineralTableIndex], product.MineralTableWidth, item.MineralTableStartIndex);


                    returnProducts.Add(GetProduct(nutrients, minerals, nutrientOneHundredGramsColumn, mineralOneHundredGramsColumn, ingredients, name, score, price));
                    Console.WriteLine("Downloaded!");
                }
            }finally
            {
                await browser.CloseAsync();
                browser.Dispose();
            }

            return returnProducts;
        }

        public static Objects.Product GetProduct(List<List<string>> nutrients, List<List<string>> minerals, int nutrientPerHundred, int mineralPerHundred, string ingredients, string name, string score, string price)
        {
            var nutrientsAsString = new StringBuilder();
            foreach (var nutrient in nutrients)
            {
                var nutrientRow = new StringBuilder();
                nutrientRow.Append('(');
                nutrientRow.Append(nutrient[0]);
                nutrientRow.Append(':');
                nutrientRow.Append(nutrient[nutrientPerHundred]);
                nutrientRow.Append(')');
                nutrientRow.Append(',');
                nutrientsAsString.Append(nutrientRow.ToString());
            }
            var mineralssAsString = new StringBuilder();
            foreach (var mineral in minerals)
            {
                var mineralRow = new StringBuilder();
                mineralRow.Append('(');
                mineralRow.Append(mineral[0]);
                mineralRow.Append(':');
                mineralRow.Append(mineral[nutrientPerHundred]);
                mineralRow.Append(')');
                mineralRow.Append(',');
                mineralssAsString.Append(mineralRow.ToString());
            }

            return new Objects.Product(
                name,
                price,
                score,
                '"' + nutrientsAsString.ToString().Replace(" & nbsp;", "") + '"',
                '"' + mineralssAsString.ToString().Replace("&nbsp;", "") + '"',
                ingredients,
                new List<string>());
        }

        public static void WriteCSV(List<Objects.Product> contents, string path)
        {
            var records = new List<string>();
            var outputFolder = path.Split('/')[0];
            System.IO.Directory.CreateDirectory(outputFolder);
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = "~" };
            using (var writer = new StreamWriter(path))
            {
                var header = "Name■Price■Score■Ingredients■Minerals■Nutrient";
                writer.WriteLine(header);
                foreach (var product in contents)
                {
                    var recordBuilder = new StringBuilder();
                    recordBuilder.Append(Regex.Replace(product.Name, @"[^0-9 . , a-zA-Z]+", ""));
                    recordBuilder.Append("■");
                    recordBuilder.Append(product.Price.Trim().Replace("\n", "").Replace("\t", ""));
                    recordBuilder.Append("■");
                    recordBuilder.Append(product.Score.Trim().Replace("\n", "").Replace("\t", ""));
                    recordBuilder.Append("■");
                    recordBuilder.Append(Regex.Replace(product.Ingredients, @"[^0-9,;():\s+ \n a-zA-Z]+", "").Replace("\n", ""));
                    recordBuilder.Append("■");
                    recordBuilder.Append(Regex.Replace(product.MineralInformation, @"[^0-9,;():\s+ a-zA-Z]+", ""));
                    recordBuilder.Append("■");
                    recordBuilder.Append(Regex.Replace(product.NutrientInformation, @"[^0-9,;():\s+ \n a-zA-Z]+", ""));
                    records.Add(recordBuilder.ToString());
                    Console.WriteLine(recordBuilder.ToString());
                    writer.WriteLine(recordBuilder.ToString());
                }
            }
            Console.WriteLine($"{records.Count} have been written to {path}");
        }
    }
}