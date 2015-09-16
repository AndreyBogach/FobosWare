using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ListOfEmployees.Domain.Entities;
using ListOfEmployees.Domain.Interface;

namespace ListOfEmployees.Domain.Concrete
{
    public class EFEmployeeRepository: IEmployeeRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Employee> Employees
        {
            get { return context.Employees; }
        }

        public void SaveEmploee(Employee employee)
        {
            if (employee.EmployeeId == 0)
            {
                employee.Tax = Tax(employee.Salary);
                employee.Net = Net(employee.Salary, employee.Tax);
                context.Employees.Add(employee);
            }

            else
            {
                Employee dbEntry = context.Employees.Find(employee.EmployeeId);
                if (dbEntry != null)
                {
                    dbEntry.Name = employee.Name;
                    dbEntry.Position = employee.Position;
                    dbEntry.Status = employee.Status;
                    dbEntry.Salary = employee.Salary;
                    dbEntry.Tax = Tax(employee.Salary);
                    dbEntry.Net = Net(employee.Salary, employee.Tax);
                    dbEntry.ImageData = employee.ImageData;
                    dbEntry.ImageMimeType = employee.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Employee DeleteEmployee(int employeeId)
        {
            Employee dbEntry = context.Employees.Find(employeeId);
            if (dbEntry != null)
            {
                context.Employees.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public decimal Tax(decimal salary)
        {
            if (salary <= 10000)
            {
                return (salary*10)/100;
            }
            if (salary > 10000 && salary <= 25000)
            {
                return (salary*15)/100;
            }
            return (salary * 25) / 100;
        }

        public decimal Net(decimal salary, decimal tax)
        {
            return (salary - tax);
        }
    }
}
