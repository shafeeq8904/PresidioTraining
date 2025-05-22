using WholeApplication.Interfaces;
using WholeApplication.Misc;
using WholeApplication.Models;
using WholeApplication.Repositories;
using WholeApplication.Services;
using System.Linq;
using WholeApplication.Solid;
using static WholeApplication.Solid.Greeting;

namespace WholeApplication
{
    internal class Program
    {
        List<Employee> employees = new List<Employee>()
        {
            new Employee(101,30, "John Doe",  50000),
            new Employee(102, 25,"Jane Smith",  60000),
            new Employee(103,35, "Sam Brown",  70000)
        };
        //public delegate void MyDelegate<T>(T num1, T num2);generic type delegate
        //public delegate void MyFDelegate(float num1, float num2);
        public void Add(int n1, int n2)
        {
            int sum = n1 + n2;
            Console.WriteLine($"The sum of {n1} and {n2} is {sum}");
        }
        public void Product(int n1, int n2)
        {
            int prod = n1 * n2;
            Console.WriteLine($"The sum of {n1} and {n2} is {prod}");
        }
        Program()
        {
            //MyDelegate<int> del = new MyDelegate<int>(Add);
            Action<int, int> del = Add;
            del += Product;
            //del += delegate (int n1, int n2)
            //{
            //    Console.WriteLine($"The division result of {n1} and {n2} is {n1 / n2}");
            //};
            del += (int n1, int n2) => Console.WriteLine($"The division result of {n1} and {n2} is {n1 / n2}");
            del(100, 20);
        }
        void FindEmployee()
        {
            int empId = 102;
            //predicate is a function that returns true or false
            Predicate<Employee> predicate = e => e.Id == empId;
            //should pass the predicate to the Find method as a parameter
            Employee? emp = employees.Find(predicate);
            Console.WriteLine(emp.ToString() ?? "No such employee");
        }
        void SortEmployee()
        {
            var sortedEmployees = employees.OrderBy(e => e.Name);
            foreach (var emp in sortedEmployees)
            {
                Console.WriteLine(emp.ToString());
            }
        }
        static void Main(string[] args)
        {
            //IRepositor<int, Employee> employeeRepository = new EmployeeRepository();
            //IEmployeeService employeeService = new EmployeeService(employeeRepository);
            //ManageEmployee manageEmployee = new ManageEmployee(employeeService);
            //manageEmployee.Start();
            //new Program();
            //Program program = new();
            //program.FindEmployee();
            //program.SortEmployee();

            //single responsibility principle
            Report report = new Report();    
            report.Title = "Monthly Report";
            report.Generate();
            ReportSaver reportSaver = new ReportSaver();
            reportSaver.Save(report);
            ReportMailer reportMailer = new ReportMailer();
            reportMailer.SendEmail(report);

            //open closed principle
            //Greeter greeter = new Greeter();
            //greeter.Greet(new EnglishGreeting());
            //greeter.Greet(new SpanishGreeting());

            //Interface Segregation Principle
            IWorkable worker = new Robot();
            worker.Work();
            IWorkable worker2 = new Human();
            worker2.Work();
            IEatable worker3 = new Human();
            worker3.Eat();

        }
    }
}