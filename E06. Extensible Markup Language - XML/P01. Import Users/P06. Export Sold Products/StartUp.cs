using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage;
using ProductShop.Data;
using ProductShop.Models;
using ProductShop.ProductShop.DTO.Export;
using ProductShop.ProductShop.DTO.Import;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            string productsResult = GetSoldProducts(context);
            File.WriteAllText(@"D:\\Programming\\Entity_Framework\\XML_Processing_Exercise\\ProductShop\\ProductShop\\Results\\users-sold-products.xml", productsResult);
        }

        public static Mapper GetMapper()
        {
            var configuration = new MapperConfiguration(c => c.AddProfile<ProductShopProfile>());
            return new Mapper(configuration);
        }

        private static string Serializer<T>(T dataTransferObject, string xmlRootAttributeName)
        {
            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttributeName));

            StringBuilder productsReport = new StringBuilder();
            using StringWriter writer = new StringWriter(productsReport);

            var xmlNamespace = new XmlSerializerNamespaces();
            xmlNamespace.Add(string.Empty, string.Empty);

            serializer.Serialize(writer, dataTransferObject, xmlNamespace);

            return productsReport.ToString();
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
            ImportProductDTO[] importProductDTO = (ImportProductDTO[])xmlSerializer.Deserialize(reader);

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
            ImportCategoryDTO[] importCategoryDTO = (ImportCategoryDTO[])xmlSerialization.Deserialize(reader);

            var mapper = GetMapper();
            Category[] categories = mapper.Map<Category[]>(importCategoryDTO);

            var categoriesWithNames = categories.Where(c => c.Name != null).ToArray();
            context.Categories.AddRange(categoriesWithNames);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerilization = new XmlSerializer(typeof(ImportCategoryProductDTO[]), new XmlRootAttribute("CategoryProducts"));

            StringReader reader = new StringReader(inputXml);
            ImportCategoryProductDTO[] importCategoryProductDTO = (ImportCategoryProductDTO[])xmlSerilization.Deserialize(reader);

            var mapper = GetMapper();
            CategoryProduct[] categoryProducts = mapper.Map<CategoryProduct[]>(importCategoryProductDTO);
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                 .Select(p => new ExportProductsInRangeDTO()
                 {
                     Name = p.Name,
                     Price = p.Price,
                     BuyerName = p.Buyer.FirstName + " " + p.Buyer.LastName,
                 })
                .ToArray();

            return Serializer<ExportProductsInRangeDTO[]>(products, "Products");
        }

        public static string GetSoldProducts(ProductShopContext context) 
        {
            var mapper = GetMapper();

            var usersWithSoldItems = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ProjectTo<ExportUserWithProductDTO>(mapper.ConfigurationProvider)
                .ToArray();

            return Serializer<ExportUserWithProductDTO[]>(usersWithSoldItems, "Users");
        }
    }
}