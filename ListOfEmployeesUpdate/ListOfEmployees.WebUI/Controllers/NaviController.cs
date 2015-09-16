using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ListOfEmployees.Domain.Entities;
using ListOfEmployees.Domain.Interface;

namespace ListOfEmployees.WebUI.Controllers
{
    public class NaviController : Controller
    {
        //
        // GET: /Navi/
        private IEmployeeRepository repository;

        public NaviController(IEmployeeRepository repo)
        {
            this.repository = repo;
        }
        public PartialViewResult Menu(string status = null)
        {
            ViewBag.SelectedCategory = status;

            IEnumerable<string> categories = repository.Employees
                .Select(x => x.Status)
                .Distinct()
                .OrderBy(x => x);

            return PartialView(categories);
        }

        public PartialViewResult Filter(string position = null)
        {
            ViewBag.SelectedPosition = position;

            IEnumerable<string> positions = repository.Employees
                .Select(x => x.Position)
                .Distinct()
                .OrderBy(x => x);
            
            return PartialView(positions);
        }
    }
}
