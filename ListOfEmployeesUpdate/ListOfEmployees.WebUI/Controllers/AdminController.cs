using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using ListOfEmployees.Domain.Entities;
using ListOfEmployees.Domain.Interface;
using ListOfEmployees.WebUI.Models;
using Microsoft.Office.Interop.Excel;

namespace ListOfEmployees.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private IEmployeeRepository repository;
        public int PageSize = 15;
        public AdminController(IEmployeeRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index(string position, int page = 1)
        {
            EmployeesListViewModel model = new EmployeesListViewModel
            {
                Employees = repository.Employees
                .Where(p => position == null || p.Position == position)
                .OrderBy(p => p.EmployeeId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = position == null ?
                    repository.Employees.Count() :
                    repository.Employees.Where(e => e.Position == position).Count()
                },
                CurrentPosition = position
            };

            return View(model);
        }

        public ViewResult Edit(int employeeId)
        {
            Employee employee = repository.Employees
                .FirstOrDefault(p => p.EmployeeId == employeeId);

            return View(employee);
        }

        // Version Edit() for changes saving
        [HttpPost]
        public ActionResult Edit(Employee employee, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    employee.ImageMimeType = image.ContentType;
                    employee.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(employee.ImageData, 0, image.ContentLength);
                }
                repository.SaveEmploee(employee);
                TempData["message"] = string.Format("The changes during editing \"{0}\" were saved", employee.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Something is wrong with data values 
                return View(employee);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Employee());
        }

        [HttpPost]
        public ActionResult Delete(int employeeId)
        {
            Employee deletedEmployee = repository.DeleteEmployee(employeeId);
            if (deletedEmployee != null)
            {
                TempData["message"] = string.Format("Employee \"{0}\" was removed",
                    deletedEmployee.Name);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ExportToExcel()
        {
            Application xlApp = new Application();
            xlApp.Visible = true;

            Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = (Worksheet)wb.Worksheets[1];

            ws.Columns[1].ColumnWidth = 30;
            ws.Columns[2].ColumnWidth = 15;
            ws.Columns[3].ColumnWidth = 15;
            ws.Columns[4].ColumnWidth = 15;
            
            ws.Cells[1, 1] = "Name";
            ws.Cells[1, 2] = "Gross, UAH";
            ws.Cells[1, 3] = "Tax, UAH";
            ws.Cells[1, 4] = "Net, UAH";

            IList<Employee> emp;
            emp = repository.Employees
                .Where(m=>m.Status == "Active")
                .ToList();

            var naveSum = repository.Employees
                .Where(m=>m.Status == "Active")
                .Count();

            int counter = 2;
            decimal salary = 0;
            decimal tax = 0;
            decimal net = 0;

            foreach (Employee item in emp)
            {
                ws.Cells[counter, 1] = item.Name;
                ws.Cells[counter, 2] = item.Salary;
                ws.Cells[counter, 3] = item.Tax;
                ws.Cells[counter, 4] = item.Net;

                salary += item.Salary;
                tax += item.Tax;
                net += item.Net;

                counter ++;
            }
            ws.Cells[counter, 1] = naveSum;
            ws.Cells[counter, 2] = salary;
            ws.Cells[counter, 3] = tax;
            ws.Cells[counter, 4] = net;
            
            return RedirectToAction("Index");
        }
    }
}
