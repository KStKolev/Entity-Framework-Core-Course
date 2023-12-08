using AutoMapper;
using CarDealer.CarDealer.DTO.Import;
using CarDealer.Data;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
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
            string xmlFile = File.ReadAllText(@"D:\Programming\Entity_Framework\XML_Processing_Exercise\CarDealer\CarDealer\Datasets\customers.xml");
            Console.WriteLine(ImportCustomers(context, xmlFile));
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

        public static string ImportParts(CarDealerContext context, string inputXml) 
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportPartsDTO[]), new XmlRootAttribute("Parts"));

            using StringReader reader = new StringReader(inputXml);
            ImportPartsDTO[] partsDTO = (ImportPartsDTO[]) xmlSerializer.Deserialize(reader);

            var map = GetMapper();

            var suppliers = context.Suppliers
            .Select(s => s.Id)
            .ToList();

            HashSet<Part> parts = new();

            foreach (var part in partsDTO)
            {
                if (suppliers.Contains(part.SupplierId))
                {
                    Part containedPart = map.Map<Part>(part);
                    parts.Add(containedPart);
                }
            }
            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml) 
        {
            var xmlSerialization = new XmlSerializer(typeof(ImportCarsDTO[]), new XmlRootAttribute("Cars"));
            using StringReader reader = new StringReader(inputXml);
            ImportCarsDTO[] carsDTO = (ImportCarsDTO[]) xmlSerialization.Deserialize(reader);


            var map = GetMapper();
            List<Car> cars = new List<Car>();
            foreach (var carDTO in carsDTO)
            {
                Car car = map.Map<Car>(carDTO);

                var partsId = carDTO.Parts
                .Select(p => p.PartId)
                .Distinct()
                .ToArray();

                List<PartCar> carParts = new List<PartCar>();

                foreach (var part in partsId)
                {
                    carParts.Add(new PartCar 
                    {
                        Car = car,
                        PartId = part
                    });
                }
                car.PartsCars = carParts;
                cars.Add(car);
            }

            context.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml) 
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportCustomersDTO[]), new XmlRootAttribute("Customers"));

            using StringReader reader = new StringReader(inputXml);
            ImportCustomersDTO[] customerDTOs = (ImportCustomersDTO[]) xmlSerializer.Deserialize(reader);
            var map = GetMapper();
            Customer[] customers = map.Map<Customer[]>(customerDTOs);
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count()}";
        }
    }
}