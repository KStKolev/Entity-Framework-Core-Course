using AutoMapper;
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
            string filePath = "D:\\Programming\\Entity_Framework\\XML_Processing_Exercise\\ProductShop\\ProductShop\\Datasets\\users.xml";
            var xmlReadFile = File.ReadAllText(filePath);
            Console.WriteLine(ImportUsers(context, xmlReadFile));
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
            ImportUserDTO[] importUsers = (ImportUserDTO[]) xmlSerializer.Deserialize(stringReader);

            var mapper = GetMapper();
            User[] users = mapper.Map<User[]>(importUsers);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }
    }
}