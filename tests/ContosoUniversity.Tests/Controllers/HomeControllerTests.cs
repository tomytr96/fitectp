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
    public class HomeControllerTests : IntegrationTestsBase
    {
        private MockHttpContextWrapper httpContext;
        private AccountController controllerToTest;
        private SchoolContext dbContext;

        //[TearDown]
        //public void RemoveNewDatabase()
        //{
        //    SettingUpTests();
        //}

        private readonly PersonVM loginInstructor = new PersonVM()
        {
            FirstMidName = "Nourhene",
            LastName = "BAOUAB",
            Username = "nour",
            Password = "nour",
            ConfirmPassword = "nour",
            Role = "Instructor"
        };

        //Arrange
        private readonly PersonVM registerStudent = new PersonVM()
        {
            FirstMidName = "Clement",
            LastName = "GUITTET",
            Username = "clem",
            Password = "123",
            ConfirmPassword = "123",
            Role = "Student"
        };
        private readonly PersonVM loginStudent = new PersonVM()
        {
            FirstMidName = "Tomy",
            LastName = "TRIMOREAU",
            Username = "toto",
            Password = "123",
            ConfirmPassword = "123",
            Role = "Student"
        };

        private readonly PersonVM registerInstructor = new PersonVM()
        {
            FirstMidName = "Younes",
            LastName = "DRIS",
            Username = "youn",
            Password = "123",
            ConfirmPassword = "123",
            Role = "Instructor"
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
        public void StudentRegister_Student_ViewLogin()
        {
            //Act
            RedirectToRouteResult result = controllerToTest.Register(registerStudent) as RedirectToRouteResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Login"));
        }

        #region public void InstructorRegister_Instructor_ViewLogin()
        [Test]
        public void InstructorRegister_Instructor_ViewLogin()
        {
            //Act
            RedirectToRouteResult result = controllerToTest.Register(registerInstructor) as RedirectToRouteResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Login"));
        } 
        #endregion

        [Test]
        public void StudentLogin_Student_ViewStudentIndex()
        {
            //Act
            RedirectToRouteResult result = controllerToTest.Login(loginStudent.Username, loginStudent.Password) as RedirectToRouteResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        #region public void InstructorLogin_Instructor_ViewInstructorIndex()
        [Test]
        public void InstructorLogin_Instructor_ViewInstructorIndex()
        {
            RedirectToRouteResult result = controllerToTest.Login(loginInstructor.Username, loginInstructor.Password) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }
        #endregion


    }
}
