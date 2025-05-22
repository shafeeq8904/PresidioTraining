using c__Day_14.Handlers;
using c__Day_14.Interfaces;
using c__Day_14.Services;

namespace EmployeeApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEmployeeService service = new EmployeeService();
            var handler = new EmployeeHandler(service);

            while (true)
            {
                Console.WriteLine("1. Add Employee\n2. View All Employees\n3. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine() ?? "0";

                switch (choice)
                {
                    case "1":
                        handler.AddEmployee();
                        break;
                    case "2":
                        handler.ShowAllEmployees();
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option.\n");
                        break;
                }
            }
        }
    }
}