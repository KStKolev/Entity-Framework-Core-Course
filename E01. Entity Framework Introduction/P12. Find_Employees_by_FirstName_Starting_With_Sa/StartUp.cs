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
            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
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

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees.Include(e => e.EmployeesProjects).ThenInclude(ep => ep.Project)
                .FirstOrDefault(e => e.EmployeeId == 147);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var ep in employee.EmployeesProjects.OrderBy(x => x.Project.Name))
                sb.AppendLine(ep.Project.Name);

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments.Where(d => d.Employees.Count() > 5)
                .Select(e => new
                {
                    Name = e.Name,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Employees = e.Employees
                })
                .OrderBy(d => d.Employees.Count())
                .ThenBy(d => d.Name);
            StringBuilder result = new StringBuilder();
            foreach (var department in departments)
            {
                result.AppendLine($"{department.Name} - {department.ManagerFirstName} {department.ManagerLastName}");
                foreach (var employee in department.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                {
                    result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }
            return result.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context) 
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .ToArray();
            StringBuilder result = new StringBuilder();
            foreach (var project in projects)
            {
                result.AppendLine($"{project.Name}");
                result.AppendLine($"{project.Description}");
                result.AppendLine($"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt")}");
            }
            return result.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context) 
        {
            StringBuilder result = new();
            var employeesWithHigherSalary = context.Employees
                .Where(e =>
                e.Department.Name == "Engineering"
                || e.Department.Name == "Tool Design"
                || e.Department.Name == "Marketing"
                || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var employee in employeesWithHigherSalary)
            {
                employee.Salary *= 1.12m;
            }
            context.SaveChanges();

            foreach (var employee in employeesWithHigherSalary)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }
            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context) 
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();
            StringBuilder result = new();
            foreach (var employee in employees) 
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }
            return result.ToString().TrimEnd(); 
        }
    }
}
