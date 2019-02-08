using ContosoUniversity.Controllers;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.Tests.Tools;
using ContosoUniversity.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ContosoUniversity.Tests.Controllers
{
    public class AccountControllerTests : IntegrationTestsBase
    {
        private MockHttpContextWrapper httpContext;
        private AccountController controllerToTest;
        private SchoolContext dbContext;

        private readonly PersonVM student = new PersonVM()
        {
            FirstMidName = "Tomy",
            LastName = "TRIMOREAU",
            Username = "tomyTRIMOREAU",
            Password = "123",
            ConfirmPassword = "123",
            ImageFile
            ImagePath = "/Image/TomyTRIMOREAU191943976.png"
        };

        private readonly PersonVM instructor = new PersonVM()
        {
            FirstMidName = "Tomy",
            LastName = "TRIMOREAU",
            Username = "tomyTRIMOREAU",
            Password = "123",
            ImagePath = "/Image/TomyTRIMOREAU191943976.png"
        };

        [SetUp]
        public void Initialize()
        {
            httpContext = new MockHttpContextWrapper();
            controllerToTest = new AccountController();
            controllerToTest.ControllerContext = new ControllerContext(httpContext.Context.Object, new RouteData(), controllerToTest);
            dbContext = new DAL.SchoolContext(this.ConnectionString);
            controllerToTest.DbContext = dbContext;
        }

        [Test]
        public void StudentRegister_Student_True()
        {
            

            string expectedLastName = "TRIMOREAU";
            string expectedFirstName = "Tomy";

            EntityGenerator generator = new EntityGenerator(dbContext);
            var result = controllerToTest.Register(student);

            //var resultModel = result.Model as Student;

            Assert.AreEqual(expectedFirstName, student.FirstMidName);
            Assert.AreEqual(expectedLastName, student.LastName);


        }
    }
}
