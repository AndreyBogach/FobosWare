using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ListOfEmployees.Domain.Entities;
using ListOfEmployees.Domain.Interface;
using ListOfEmployees.WebUI.Controllers;
using ListOfEmployees.WebUI.HtmlHelpers;
using ListOfEmployees.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ListOfEmployees.UnitTests
{
    [TestClass]
    public class EmployeeUnitTest
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1"},
                new Employee {EmployeeId = 2, Name = "Employee2"},
                new Employee {EmployeeId = 3, Name = "Employee3"},
                new Employee {EmployeeId = 4, Name = "Employee4"},
                new Employee {EmployeeId = 5, Name = "Employee5"},
            }.AsQueryable());
            
            EmployeeController controller = new EmployeeController(mock.Object);

            controller.PageSize = 3;

            EmployeesListViewModel result = (EmployeesListViewModel)controller.List(null, 2).Model;

            Employee[] prodArray = result.Employees.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "Employee4");
            Assert.AreEqual(prodArray[1].Name, "Employee5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper helper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = helper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1"},
                new Employee {EmployeeId = 2, Name = "Employee2"},
                new Employee {EmployeeId = 3, Name = "Employee3"},
                new Employee {EmployeeId = 4, Name = "Employee4"},
                new Employee {EmployeeId = 5, Name = "Employee5"},
            }.AsQueryable());

            EmployeeController controller = new EmployeeController(mock.Object);

            controller.PageSize = 3;

            EmployeesListViewModel result = (EmployeesListViewModel)controller.List(null, 2).Model;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Employees()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1", Status = "Active"},
                new Employee {EmployeeId = 2, Name = "Employee2", Status = "Inactive"},
                new Employee {EmployeeId = 3, Name = "Employee3", Status = "Active"},
                new Employee {EmployeeId = 4, Name = "Employee4", Status = "Inactive"},
                new Employee {EmployeeId = 5, Name = "Employee5", Status = "Active"},
            }.AsQueryable());

            EmployeeController controller = new EmployeeController(mock.Object);

            controller.PageSize = 3;

            Employee[] result = ((EmployeesListViewModel)controller.List("Inactive", 1).Model)
               .Employees.ToArray();

            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "Employee2" && result[0].Status == "Inactive");
            Assert.IsTrue(result[1].Name == "Employee4" && result[1].Status == "Inactive");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange
            // - create the mock repository
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1", Status = "Active"},
                new Employee {EmployeeId = 2, Name = "Employee2", Status = "Active"},
                new Employee {EmployeeId = 3, Name = "Employee3", Status = "Inactive"},
                new Employee {EmployeeId = 4, Name = "Employee4", Status = "Inactive"},
            }.AsQueryable());

            // Arrange - create the controller
            NaviController target = new NaviController(mock.Object);

            // Act = get the set of categories
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0], "Active");
            Assert.AreEqual(results[1], "Inactive");
        }

        [TestMethod]
        public void Indicates_Selected_Position()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1", Status = "Active"},
                new Employee {EmployeeId = 4, Name = "Employee2", Status = "Inactive"},
            }.AsQueryable);

            NaviController target = new NaviController(mock.Object);

            string categoryToSelect = "Active";

            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Employees_Count()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1", Status = "Active"},
                new Employee {EmployeeId = 2, Name = "Employee2", Status = "Inactive"},
                new Employee {EmployeeId = 3, Name = "Employee3", Status = "Active"},
                new Employee {EmployeeId = 4, Name = "Employee4", Status = "Inactive"},
                new Employee {EmployeeId = 5, Name = "Employee5", Status = "Inactive"},
            }.AsQueryable);

            EmployeeController target = new EmployeeController(mock.Object);
            target.PageSize = 3;

            int res1 = ((EmployeesListViewModel)target.List("Active").Model).PagingInfo.TotalItems;
            int res2 = ((EmployeesListViewModel)target.List("Inactive").Model).PagingInfo.TotalItems;
            int resAll = ((EmployeesListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 3);
            Assert.AreEqual(resAll, 5);
        }
    }
}
