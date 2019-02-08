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
            FirstMidName = "Nourhene ",
            LastName = "Baouab",
            Username = "Nourhene",
            Password = "123",
            ImagePath = "/Image/totoTOTO194946355.png"
        };
        private readonly PersonVM Instructor = new PersonVM()
        {
            FirstMidName = "Nourhene ",
            LastName = "Baouab",
            Username = "Nourhene",
            Password = "123",
            ImagePath = "/Image/totoTOTO194946355.png"
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
        public void StudentRegister_student_True()
        {
            string expectedLastName = "Baouab";
            string expectedFirstName = "Nourhene";

            EntityGenerator generator = new EntityGenerator(dbContext);
            //Student student = generator.CreateStudent(expectedLastName, expectedFirstName);
            //SECOND PARAMETRE A REMPLACER
            var result = controllerToTest.Register(student) as ViewResult;

            var resultModel = result.Model as Student;

            Assert.That(result, Is.Not.Null);
            Assert.That(resultModel, Is.Not.Null);
            Assert.That(expectedLastName, Is.EqualTo(resultModel.LastName));
            Assert.That(expectedFirstName, Is.EqualTo(resultModel.FirstMidName));
        }


    }
}
