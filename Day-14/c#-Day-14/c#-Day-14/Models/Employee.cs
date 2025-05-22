using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c__Day_14.Models
{
    public class Employee
    {
        public int Id { get; }
        public string Name { get; }

        public Employee(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => $"ID: {Id}, Name: {Name}";
    }
}
