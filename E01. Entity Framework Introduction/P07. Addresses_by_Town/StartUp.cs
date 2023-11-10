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

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.EmployeesProjects.Any(ep =>
                    ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new {
                    e.FirstName,
                    e.LastName,
                    e.Manager,
                    Projects = e.EmployeesProjects.Select(ep => new
                    {
                        ep.Project.Name,
                        StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                        EndDate = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished"
                    })
                })
                .ToArray();

            StringBuilder result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.Manager.FirstName} {employee.Manager.LastName}");
                foreach (var ep in employee.Projects)
                    result.AppendLine($"--{ep.Name} - {ep.StartDate} - {ep.EndDate}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context) 
        {
            var addresses = context.Addresses
                .Take(10).OrderByDescending(a => a.Employees.Count())
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText);

            StringBuilder result = new StringBuilder(); 

            foreach (var address in addresses) 
            {
                result.AppendLine($"{address.AddressText}, {address.Town.Name} - {address.Employees.Count()} employees");
            }
            return result.ToString();
        }
    }
}
