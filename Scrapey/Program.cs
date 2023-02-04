using Scrapey.Interfaces;
using Scrapey.Objects;

namespace Scrapey
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var products = new List<string>()
            //{
            //    "",
            //    "",
            //    "",
            //    "",
            //    ""
            //};
            //foreach(var product in products)
            //{
            //    ScrapeProduct(product);
            //}

            //IScraper exanteScraper = new ExanteScraper();
            //exanteScraper.Scrape("https://www.exantediet.com/weight-loss/meal-replacement-bars.list");
            //var products = exanteScraper.GetAllProductsToScrape();

            //foreach(var item in products)
            //{
            //    ScrapeProduct($"https://exantediet.com/{item}");
            //    System.Threading.Thread.Sleep(1000);
            //}
            
            var targets = new Dictionary<string, string>();

            targets["Overview"] = "productDescription_contentPropertyListItem_synopsis";
            targets["Ingredients"] = "productDescription_contentPropertyListItem_ingredients";

            var scraper = new ExanteScraper();
            scraper.Scrape("https://www.exantediet.com//diet-products/meal-replacement-lemon-bar/11213853.html");
            var result = scraper.DynamicallyGetXPathsBasedOnClass("/html/body/div[3]/div/main/div[3]/div[1]/div[2]/div[2]/div/div/div", targets);

            var mapping = new ExanteMapping(result["Overview"], result["Ingredients"], result["Price"]);
            scraper.Mapping = mapping;
            var ingredients = scraper.GetIngredients("IngredientsALLERGENS: For allergens, see ingredients in bold. INGREDIENTS:&nbsp;");
            var overview = scraper.GetOverview();
            Console.WriteLine(ingredients.Trim());
        }

        static void ScrapeProduct(string productUrl)
        {
            Console.WriteLine($"Scraping page: {productUrl}");
            IScraper exanteScraper = new ExanteScraper();
            exanteScraper.Scrape(productUrl);
            var price = exanteScraper.GetPrice();
            var name = exanteScraper.GetName();
            var ingredients = exanteScraper.GetIngredients();
            var overview = exanteScraper.GetOverview();
            var score = exanteScraper.GetScore();
            Console.WriteLine($"{price.Trim()}");
            Console.WriteLine($"{name.Trim()}");
            //Console.WriteLine($"{ingredients.Trim()}");
            //Console.WriteLine($"{overview.Trim()}");
            Console.WriteLine($"{score.Trim()}");
        }
    }
}