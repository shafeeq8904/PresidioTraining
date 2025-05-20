using System;
using System.Collections.Generic;

public class Employee  : IComparable<Employee>
{
    private int id, age;
    private string name;
    private double salary;
    public Employee() { }

    public Employee(int id, int age, string name, double salary)
    {
        this.id = id;
        this.age = age;
        this.name = name;
        this.salary = salary;
    }

    public void TakeEmployeeDetailsFromUser()
    {
        Console.WriteLine("Please enter the employee ID:");
        id = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Please enter the employee name:");
        name = Console.ReadLine();

        Console.WriteLine("Please enter the employee age:");
        age = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Please enter the employee salary:");
        salary = Convert.ToDouble(Console.ReadLine());
    }

    public override string ToString()
    {
        return "Employee ID: " + id +
               "\nName: " + name +
               "\nAge: " + age +
               "\nSalary: " + salary;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }

    public int Age
    {
        get => age;
        set => age = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public double Salary
    {
        get => salary;
        set => salary = value;
    }
    
    public int CompareTo(Employee other)
    {
        if (other == null) return 1;

        return this.salary.CompareTo(other.salary);
    }

    public class EmployeePromotion
    {
        private List<Employee> promotionList = new List<Employee>();
        private Dictionary<int, Employee> employeeDictionary = new Dictionary<int, Employee>();

        //Create a collection that will hold the employee names in the same order that they are inserted.
        public void TakeEmployeeDetails()
        {

            Console.WriteLine("Please enter the employee names ");
            Console.WriteLine("Leave blank to stop:");

            while (true)
            {
                string? name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    break;

                Employee emp = new Employee();
                emp.Name = name;
                promotionList.Add(emp);
            }

            DisplayEmployeeNames();

            Console.WriteLine($"The current size of the collection is: {promotionList.Capacity}");

            promotionList.TrimExcess();

            Console.WriteLine($"The size after removing the extra space is: {promotionList.Capacity}");

        }

        //display the employee names in the order they were entered
        public void DisplayEmployeeNames()
        {
            Console.WriteLine("\n--- Employee Names (in entry order) ---");
            foreach (var emp in promotionList)
            {
                Console.WriteLine(emp.Name);
            }
        }

        //Given an employee name find his position in the promotion list
        public void DisplayPromotionList()
        {
            Console.WriteLine("\n--- Promotion List (in entry order) ---");
            string nameToCheck = Console.ReadLine();

            int index = promotionList.FindIndex(emp => emp.Name.Equals(nameToCheck, StringComparison.OrdinalIgnoreCase));
            if (index != -1)
            {
                Console.WriteLine($"{nameToCheck} is in the list of promoted employees at position of {index + 1}.");
            }
            else
            {
                Console.WriteLine($"{nameToCheck} is not in the list of promoted employees.");
            }
        }

        //Sort the employee names in the promotion list in ascending order
        public void DisplaySortedPromotionList()
        {
            Console.WriteLine("\nPromoted employee list:");

            if (promotionList.Count == 0)
            {
                Console.WriteLine("No employees in the promotion list.");
                return;
            }

            var sorted = promotionList.OrderBy(emp => emp.Name).ToList();

            foreach (var emp in sorted)
            {
                Console.WriteLine(emp.Name);
            }
        }

        //Add employee full details
        //ID, Name, Age, Salary
        public void AddEmployeeFullDetails()
        {
            Console.WriteLine("Enter full employee details. blank enter to stop:");

            while (true)
            {
                Console.Write("Enter Employee ID: ");
                string idInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(idInput))
                    break;

                if (!int.TryParse(idInput, out int id) || id <= 0)
                {
                    Console.WriteLine("Invalid ID. Try again.");
                    continue;
                }

                if (employeeDictionary.ContainsKey(id))
                {
                    Console.WriteLine("Duplicate ID. Each employee must have a unique ID.");
                    continue;
                }

                Employee emp = new Employee();
                emp.Id = id;

                Console.Write("Enter Name: ");
                emp.Name = Console.ReadLine();

                Console.Write("Enter Age: ");
                if (!int.TryParse(Console.ReadLine(), out int age))
                {
                    Console.WriteLine("Invalid age. Skipping this employee.");
                    continue;
                }
                emp.Age = age;

                Console.Write("Enter Salary: ");
                if (!double.TryParse(Console.ReadLine(), out double salary))
                {
                    Console.WriteLine("Invalid salary. Skipping this employee.");
                    continue;
                }
                emp.Salary = salary;

                employeeDictionary.Add(id, emp);
                Console.WriteLine("Employee added successfully.\n");

            }
        }

        //Find employee by ID
        //If the employee is found, display all his details
        public void FindEmployeeById()
        {
            Console.Write("Enter Employee ID to search: ");
            int id = Convert.ToInt32(Console.ReadLine());

            if (id <= 0)
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var result = from e in employeeDictionary.Values
                         where e.Id == id
                         select e;

            if (result.Any())
            {
                foreach (var emp in result)
                    Console.WriteLine("Employee found:\n" + emp);
            }
            else
            {
                Console.WriteLine("No employee found with that ID.");
            }
        }

        //Sort the employees by salary in ascending order by using the IComparable interface
        public void SortEmployeesBySalary()
        {
            if (employeeDictionary.Count == 0)
            {
                Console.WriteLine("No employees to sort.");
                return;
            }

            List<Employee> employeeList = new List<Employee>(employeeDictionary.Values);
            employeeList.Sort();

            Console.WriteLine("\n--- Employees Sorted by Salary ---");
            foreach (var emp in employeeList)
            {
                Console.WriteLine(emp);
                Console.WriteLine("-----------------------------");
            }
        }

        //Find employees by name
        public void FindEmployeesByName()
        {
            Console.Write("Enter the employee name to search: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Invalid name. Please try again.");
                return;
            }

            var matchedEmployees = employeeDictionary.Values.Where(emp => emp.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchedEmployees.Count == 0)
            {
                Console.WriteLine("No employees found with that name.");
            }
            else
            {
                Console.WriteLine($"\nFound {matchedEmployees.Count} employee(s) with the name '{name}':");
                foreach (var emp in matchedEmployees)
                {
                    Console.WriteLine(emp);
                    Console.WriteLine("-----------------------------");
                }
            }
        }

        //Find employees older than a given employee
        public void FindEmployeesOlderThanGiven()
        {
            Console.Write("Enter the employee ID to compare age: ");
            if (!int.TryParse(Console.ReadLine(), out int refId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            if (!employeeDictionary.TryGetValue(refId, out Employee? refEmployee))
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            var olderEmployees = employeeDictionary.Values
                .Where(emp => emp.Age > refEmployee.Age)
                .ToList();

            if (olderEmployees.Count == 0)
            {
                Console.WriteLine($"No employees are older than {refEmployee.Name} (Age: {refEmployee.Age}).");
            }
            else
            {
                Console.WriteLine($"\nEmployees older than {refEmployee.Name} (Age: {refEmployee.Age}):");
                foreach (var emp in olderEmployees)
                {
                    Console.WriteLine(emp);
                    Console.WriteLine("-----------------------------");
                }
            }
        }

        public void PrintAllEmployees()
        {
            if (employeeDictionary.Count == 0)
            {
                Console.WriteLine("No employees available.");
                return;
            }

            Console.WriteLine("\n--- All Employee Details ---");
            foreach (var emp in employeeDictionary.Values)
            {
                Console.WriteLine(emp);
                Console.WriteLine("-----------------------------");
            }
        }

        //delete employee by ID
        public void DeleteEmployeeById()
        {
            Console.Write("Enter the employee ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            if (employeeDictionary.Remove(id))
            {
                Console.WriteLine("Employee deleted successfully.");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }

        //modify employee details
        public void ModifyEmployeeDetails()
        {
            Console.Write("Enter the employee ID to modify: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            if (!employeeDictionary.TryGetValue(id, out Employee? emp))
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            Console.WriteLine($"Modifying details for Employee ID {id}");

            Console.Write("Enter new Name (leave blank to keep existing): ");
            string? nameInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nameInput))
                emp.Name = nameInput;

            Console.Write("Enter new Age (leave blank to keep existing): ");
            string? ageInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ageInput) && int.TryParse(ageInput, out int newAge))
                emp.Age = newAge;

            Console.Write("Enter new Salary (leave blank to keep existing): ");
            string? salaryInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(salaryInput) && double.TryParse(salaryInput, out double newSalary))
                emp.Salary = newSalary;

            Console.WriteLine("Employee details updated.");
        }



        public static void Main()
        {
            EmployeePromotion app = new EmployeePromotion();
            while (true)
            {
                Console.WriteLine("\nEmployee Promotion Management System");
                Console.WriteLine("1 - Enter employee for promotion");
                Console.WriteLine("2 - Find employee promotion");
                Console.WriteLine("3 - Display sorted promotion list");
                Console.WriteLine("4 - Add employee full details");
                Console.WriteLine("5 - Find employee by ID");
                Console.WriteLine("6 - Sort employees by salary");
                Console.WriteLine("7 - Find employees by name");
                Console.WriteLine("8 - Find employees older than given employee");
                Console.WriteLine("9 - Print all employees");
                Console.WriteLine("10 - Delete employee by ID");
                Console.WriteLine("11 - Modify employee details");
                Console.WriteLine("12 - Exit");
                Console.Write("Enter your choice: ");

                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        app.TakeEmployeeDetails();
                        break;
                    case 2:
                        app.DisplayPromotionList();
                        break;
                    case 3:
                        app.DisplaySortedPromotionList();
                        break;
                    case 4:
                        app.AddEmployeeFullDetails();
                        break;
                    case 5:
                        app.FindEmployeeById();
                        break;
                    case 6:
                        app.SortEmployeesBySalary();
                        break;
                    case 7:
                        app.FindEmployeesByName();
                        break;
                    case 8:
                        app.FindEmployeesOlderThanGiven();
                        break;
                    case 9:
                        app.PrintAllEmployees();
                        break;
                    case 10:
                        app.DeleteEmployeeById();
                        break;
                    case 11:
                        app.ModifyEmployeeDetails();
                        break;
                    case 12:
                        Console.WriteLine("Exiting the program.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}


