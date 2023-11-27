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
            string json = File.ReadAllText("D:\\Programming\\Entity_Framework\\ProductShop\\ProductShop\\Datasets\\categories.json");
            Console.WriteLine(ImportProducts(context, json));
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
            foreach (Product product in products)
            {
                if (product != null)
                {
                    context.Products.Add(product);
                }
            }
            context.SaveChanges();
            return $"Successfully imported {products.Count()}";
        }
    }
}