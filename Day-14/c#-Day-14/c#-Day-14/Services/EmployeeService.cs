using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c__Day_14.Interfaces;
using c__Day_14.Models;

namespace c__Day_14.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employees = new();

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        public List<Employee> GetAllEmployees() => _employees;
    }
}
