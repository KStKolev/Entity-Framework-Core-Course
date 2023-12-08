using AutoMapper;
using CarDealer.CarDealer.DTO.Import;
using CarDealer.Data;
using CarDealer.Models;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            string xmlFile = File.ReadAllText(@"D:\Programming\Entity_Framework\XML_Processing_Exercise\CarDealer\CarDealer\Datasets\suppliers.xml");
            Console.WriteLine(ImportSuppliers(context, xmlFile));
        }

        public static Mapper GetMapper() 
        {
            MapperConfiguration configuration = new MapperConfiguration(configuration => configuration.AddProfile<CarDealerProfile>());
            return new Mapper(configuration);
        }

        public static string Serialize<T>(T DTO, XmlRootAttribute root) 
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            StringBuilder result = new StringBuilder();
            StringWriter reader = new StringWriter(result);

            var xmlNamespace = new XmlSerializerNamespaces();
            xmlNamespace.Add(string.Empty, string.Empty);

            serializer.Serialize(reader, DTO, xmlNamespace);

            return result.ToString();
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml) 
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportSuppliersDTO[]), new XmlRootAttribute("Suppliers"));
            StringReader reader = new StringReader(inputXml);
            ImportSuppliersDTO[] supplierDTO = (ImportSuppliersDTO[])xmlSerializer.Deserialize(reader);

            var map = GetMapper();
            Supplier[] suppliers = map.Map<Supplier[]>(supplierDTO);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}";
        }
    }
}