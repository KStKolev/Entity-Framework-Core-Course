using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using ProductShop.Data;
using ProductShop.Models;
using ProductShop.ProductShop.DTO.Import;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            string filePath = "D:\\Programming\\Entity_Framework\\XML_Processing_Exercise\\ProductShop\\ProductShop\\Datasets\\categories.xml";
            var xmlReadFile = File.ReadAllText(filePath);
            Console.WriteLine(ImportCategories(context, xmlReadFile));
        }

        public static Mapper GetMapper()
        {
            var configuration = new MapperConfiguration(c => c.AddProfile<ProductShopProfile>());
            return new Mapper(configuration);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportUserDTO[]), new XmlRootAttribute("Users"));

            using StringReader stringReader = new StringReader(inputXml);
            ImportUserDTO[] importUsers = (ImportUserDTO[])xmlSerializer.Deserialize(stringReader);

            var mapper = GetMapper();
            User[] users = mapper.Map<User[]>(importUsers);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml) 
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportProductDTO[]), new XmlRootAttribute("Products"));

            StringReader reader = new StringReader(inputXml);
            ImportProductDTO[] importProductDTO = (ImportProductDTO[]) xmlSerializer.Deserialize(reader);

            foreach (ImportProductDTO product in importProductDTO)
            {
                product.BuyerId = product.BuyerId == 0 ? null : product.BuyerId;
            }

            var mapper = GetMapper();
            Product[] products = mapper.Map<Product[]>(importProductDTO);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml) 
        {
            var xmlSerialization = new XmlSerializer(typeof(ImportCategoryDTO[]), new XmlRootAttribute("Categories"));

            StringReader reader = new StringReader(inputXml);
            ImportCategoryDTO[] importCategoryDTO = (ImportCategoryDTO[]) xmlSerialization.Deserialize(reader);

            var mapper = GetMapper();
            Category[] categories = mapper.Map<Category[]>(importCategoryDTO);

            var categoriesWithNames = categories.Where(c => c.Name != null).ToArray();
            context.Categories.AddRange(categoriesWithNames);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }
    }
}