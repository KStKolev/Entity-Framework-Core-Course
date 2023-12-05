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
            var suppliedParts = parts.Where(p => p.SupplierId != null).ToArray();
            context.Parts.AddRange(suppliedParts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count()}.";
        }
    }
}