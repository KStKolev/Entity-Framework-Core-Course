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
    }
}