using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ContosoUniversity.Controllers
{

    public class AccountController : Controller
    {
        private SchoolContext db = new SchoolContext();
        public SchoolContext DbContext
        {
            get { return db; }
            set { db = value; }
        }
        // GET: Account
        public ActionResult Index()
        {

            return View();

        }
        public ActionResult Register()
        {
            TempData["ErrorMessage"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult Register(PersonVM model)
        {
            if (model.Password == model.ConfirmPassword)
            {
                string fileName = model.FirstMidName + model.LastName.ToUpper();
                string extension = Path.GetExtension(model.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                model.ImagePath = "/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("/Image/"), fileName);
                model.ImageFile.SaveAs(fileName);
                using (SchoolContext db = new SchoolContext())
                {

                    if (!db.People.Any(u => u.Username == model.Username) && model.Role == "Student")
                    {
                        Student user = new Student
                        {
                            FirstMidName = model.FirstMidName,
                            LastName = model.LastName,
                            Username = model.Username,
                            Password = model.Password,
                            EnrollmentDate = DateTime.Now,
                            ImagePath = model.ImagePath
                        };
                        db.Students.Add(user);
                        db.SaveChanges();
                        ViewBag.Message = "Welcome " + user.Username;
                        return RedirectToAction("Login");

                    }
                    else if (!db.People.Any(u => u.Username == model.Username) && model.Role == "Instructor")
                    {
                        {
                            Instructor user = new Instructor
                            {
                                FirstMidName = model.FirstMidName,
                                LastName = model.LastName,
                                Username = model.Username,
                                Password = model.Password,
                                HireDate = DateTime.Now,
                                ImagePath = model.ImagePath
                            };
                            db.Instructors.Add(user);
                            db.SaveChanges();
                            ViewBag.Message = "Welcome " + user.Username;
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Username already exists";
                        return View();
                    }
                }
            }
            TempData["ErrorMessage"] = "Password Not Valid";
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string username, string password) //TODO : Ici ça serait bien de mettre un ViewModel ...
        {
            using (SchoolContext db = new SchoolContext())
            {
                if (db.People.FirstOrDefault(u => u.Username == username && u.Password == password) is Student)

                {
                    Student user = new Student();
                    user.Username = username;
                    user.Password = password;
                    Session["UserID"] = user;
                    TempData["LoginMessage"] = "Welcome " + username;
                    return RedirectToAction("Index", "Student");
                }
                else if (db.People.FirstOrDefault(u => u.Username == username && u.Password == password) is Instructor)
                {
                    Instructor user = new Instructor();
                    user.Username = username;
                    user.Password = password;
                    Session["UserID"] = user;
                    TempData["LoginMessage"] = "Welcome " + username;
                    return RedirectToAction("Index", "Instructor");

                }
                else { ModelState.AddModelError("", "Username or Password is wrong"); }



            }
            return View();

        }

        public ActionResult Logout()
        {
            if (Session["UserID"] != null)
            {

                FormsAuthentication.SignOut();
            }


            return RedirectToAction("Index", "Home");
        }


    }

}






