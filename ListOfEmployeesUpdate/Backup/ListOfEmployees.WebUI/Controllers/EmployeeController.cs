using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ListOfEmployees.Domain.Entities;
using ListOfEmployees.Domain.Interface;
using ListOfEmployees.WebUI.Models;

namespace ListOfEmployees.WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        private IEmployeeRepository repository;

        public int PageSize = 10;
        public EmployeeController(IEmployeeRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Home()
        {
            return View();
        }

        public ViewResult Contact()
        {
            return View();
        }

        public ViewResult List(string status, int page = 1)
        {
            EmployeesListViewModel model = new EmployeesListViewModel
            {
                Employees = repository.Employees
                .Where(p => status == null || p.Status == status)
                .OrderBy(p => p.EmployeeId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = status == null ?
                    repository.Employees.Count() :
                    repository.Employees.Where(e => e.Status == status).Count()
                },
                CurrentStatus = status
            };

            return View(model);
        }

        public FileContentResult GetImage(int employeeId)
        {
            Employee emp = repository.Employees.FirstOrDefault(p => p.EmployeeId == employeeId);

            if (emp != null)
            {
                return File(emp.ImageData, emp.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}
