using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListOfEmployees.Domain.Entities;

namespace ListOfEmployees.Domain.Concrete
{
    class EFDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; } 
    }
}
