using CarDealer.Data;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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

        public static string ImportSales(CarDealerContext context, string inputJson) 
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
            {
                c.Name,
                BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                c.IsYoungDriver
            })
            .ToArray();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context) 
        {
            var cars = context.Cars.Where(c => c.Make.Equals("Toyota"))
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TraveledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ToArray();

            string json = JsonConvert.SerializeObject (cars, Formatting.Indented);
            return json;
        }
    }
}