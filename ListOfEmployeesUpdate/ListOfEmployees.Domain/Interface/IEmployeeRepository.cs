using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListOfEmployees.Domain.Entities;

namespace ListOfEmployees.Domain.Interface
{
    public interface IEmployeeRepository
    {
        IQueryable<Employee> Employees { get; }
        void SaveEmploee (Employee employee);
        Employee DeleteEmployee(int employeeId);
    }
}
