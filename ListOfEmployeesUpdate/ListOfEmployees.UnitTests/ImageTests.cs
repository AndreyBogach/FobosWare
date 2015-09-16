using System;
using System.Linq;
using System.Web.Mvc;
using ListOfEmployees.Domain.Entities;
using ListOfEmployees.Domain.Interface;
using ListOfEmployees.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ListOfEmployees.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            Employee prod = new Employee
            {
                EmployeeId = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "P1"},
                prod,
                new Employee {EmployeeId = 3, Name = "P3"}, 
            }.AsQueryable);

            EmployeeController target = new EmployeeController(mock.Object);

            ActionResult result = target.GetImage(2);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            Mock<IEmployeeRepository> mock = new Mock<IEmployeeRepository>();
            mock.Setup(m => m.Employees).Returns(new Employee[]
            {
                new Employee {EmployeeId = 1, Name = "P1"},
                new Employee {EmployeeId = 2, Name = "P2"}, 
            }.AsQueryable);

            EmployeeController target = new EmployeeController(mock.Object);

            ActionResult result = target.GetImage(100);

            Assert.IsNull(result);
        }
    }
}
