using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Diagnostics.Metrics;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main() 
        {
            SoftUniContext context = new SoftUniContext();
            Console.WriteLine(AddNewAddressToEmployee(context));
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            DbSet<Employee> employees = context.Employees;
            
            string result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}"));
            return result;
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context) 
        {
            StringBuilder result = new StringBuilder();
            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName);

            foreach (Employee employee in employees)
            {
                result.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return result.ToString();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context) 
        {
            StringBuilder result = new StringBuilder();
            var employees = context.Employees.Where(e => e.Department.Name == "Research and Development");
            foreach (var employee in employees.OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName))
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary:F2}");
            }
            return result.ToString();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context) 
        {
            Address address = new Address();
            address.AddressText = "Vitoshka 15";
            address.TownId = 4;

            var employeeNakov = context.Employees
                .First(e => e.LastName == "Nakov");
            employeeNakov.Address = address;
            context.SaveChanges();
            var employees = context.Employees
                .Select(e => new {e.AddressId, e.Address.AddressText })
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .ToList();

            return string.Join(Environment.NewLine, employees.Select(e => $"{e.AddressText}"));
        }
    }
}
