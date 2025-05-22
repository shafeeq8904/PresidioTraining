using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c__Day_14.Models;

namespace c__Day_14.Interfaces
{
    public interface IEmployeeService
    {
        void AddEmployee(Employee employee);
        List<Employee> GetAllEmployees();
    }
}
