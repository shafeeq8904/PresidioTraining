using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c__Day_14.Interfaces;
using c__Day_14.Models;

namespace c__Day_14.Handlers
{
    public class EmployeeHandler
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public void AddEmployee()
        {
            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter Name: ");
            string name = Console.ReadLine() ?? "Unknown";

            _employeeService.AddEmployee(new Employee(id, name));
            Console.WriteLine("Employee added successfully.\n");
        }

        public void ShowAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            Console.WriteLine("\nEmployee List:");
            foreach (var emp in employees)
            {
                Console.WriteLine(emp);
            }
            Console.WriteLine();
        }
    }
}
