using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ListOfEmployees.Domain.Entities;
using ListOfEmployees.Domain.Interface;
using ListOfEmployees.WebUI.Controllers;
using ListOfEmployees.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ListOfEmployees.UnitTests
{
    [TestClass]
    public class AdminUnitTest
    {
        [TestMethod]
        public void Index_Contains_All_Employees()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1"},
                new Employee {EmployeeId = 2, Name = "Employee2"},
                new Employee {EmployeeId = 3, Name = "Employee3"}, 
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            target.PageSize = 3;

            Employee[] result = ((EmployeesListViewModel)target.Index(null).ViewData.Model).Employees.ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("Employee1", result[0].Name);
            Assert.AreEqual("Employee2", result[1].Name);
            Assert.AreEqual("Employee3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Employee()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1"},
                new Employee {EmployeeId = 2, Name = "Employee2"},
                new Employee {EmployeeId = 3, Name = "Employee3"}, 
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Employee p1 = target.Edit(1).ViewData.Model as Employee;
            Employee p2 = target.Edit(2).ViewData.Model as Employee;
            Employee p3 = target.Edit(3).ViewData.Model as Employee;

            Assert.AreEqual(1, p1.EmployeeId);
            Assert.AreEqual(2, p2.EmployeeId);
            Assert.AreEqual(3, p3.EmployeeId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1"},
                new Employee {EmployeeId = 2, Name = "Employee2"},
                new Employee {EmployeeId = 3, Name = "Employee3"}, 
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Employee result = (Employee)target.Edit(4).ViewData.Model;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();

            AdminController target = new AdminController(mock.Object);

            Employee product = new Employee { Name = "Test" };

            ActionResult result = target.Edit(product, null);

            mock.Verify(m => m.SaveEmploee(product));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();

            AdminController target = new AdminController(mock.Object);

            Employee product = new Employee { Name = "Test" };

            target.ModelState.AddModelError("error", "error");

            ActionResult result = target.Edit(product, null);

            mock.Verify(m => m.SaveEmploee(It.IsAny<Employee>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Employees()
        {
            Employee prod = new Employee { EmployeeId = 2, Name = "Test" };

            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();

            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "Employee1"},
                prod,
                new Employee {EmployeeId = 3, Name = "Employee3"}, 
            }.AsQueryable);

            AdminController target = new AdminController(mock.Object);

            target.Delete(prod.EmployeeId);

            mock.Verify(m => m.DeleteEmployee(prod.EmployeeId));
        }
    }
}
