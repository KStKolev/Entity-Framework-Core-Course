using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            string json = File.ReadAllText("D:\\Programming\\Entity_Framework\\ProductShop\\ProductShop\\Datasets\\products.json");
            Console.WriteLine(GetProductsInRange(context));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson) 
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson) 
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);
            context.SaveChanges();
            return $"Successfully imported {products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson) 
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);
            var validCategories = categories
                .Where(c => c.Name != null)
                .ToArray();

            context.Categories.AddRange(validCategories);
            context.SaveChanges();
            return $"Successfully imported {validCategories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            context.CategoriesProducts.AddRange(categoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context) 
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new 
                {
                    name = p.Name,
                    price = p.Price,
                    seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .OrderBy(p => p.price)
                .ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);
            return json;
        }
    }
}