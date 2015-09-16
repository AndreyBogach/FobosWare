using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ListOfEmployees.Domain.Entities;

namespace ListOfEmployees.WebUI.Models
{
    public class EmployeesListViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentPosition { get; set; }
    }
}