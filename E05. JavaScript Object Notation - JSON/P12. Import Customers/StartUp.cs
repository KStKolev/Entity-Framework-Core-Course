using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {

        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson) 
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson) 
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson);
            var suppliedParts = parts
                .Where(p => p.SupplierId != null)
                .ToArray();
            context.Parts.AddRange(suppliedParts);
            context.SaveChanges();
            return $"Successfully imported {suppliedParts.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson) 
        {
            var cars = JsonConvert.DeserializeObject<Car[]>(inputJson);
            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count()}."; ;
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson) 
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count()}."; ;
        }
    }
}