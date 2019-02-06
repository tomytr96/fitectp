using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ContosoUniversity.Controllers
{

    public class AccountController : Controller
    {
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
        public ActionResult Register(string FirstMidName, string LastName, string Role, string Username, string Password, string ConfirmPassword)
        {
            //if (ModelState.IsValid)
            if (Password == ConfirmPassword)
            {

                using (SchoolContext db = new SchoolContext())
                {

                    if (!db.People.Any(u => u.Username == Username) && Role == "Student")
                    {
                        Student user = new Student();
                        user.FirstMidName = FirstMidName;
                        user.LastName = LastName;
                        user.Username = Username;
                        user.Password = Password;
                        user.EnrollmentDate = DateTime.Now;
                        db.Students.Add(user);
                        db.SaveChanges();
                        ViewBag.Message = "Welcome " + Username;
                        return RedirectToAction("Login");

                    }
                    else if (!db.People.Any(u => u.Username == Username) && Role == "Instructor")
                    {
                        {
                            Instructor user = new Instructor();
                            user.FirstMidName = FirstMidName;
                            user.LastName = LastName;
                            user.Username = Username;
                            user.Password = Password;
                            user.HireDate = DateTime.Now;
                            db.Instructors.Add(user);
                            db.SaveChanges();
                            ViewBag.Message = "Welcome " + Username;
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Username already exists";
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
            //if (Session["UserID"] != null)
            //{

            //    FormsAuthentication.SignOut();
            //}
            Session["UserID"] = null;

            return RedirectToAction("Index", "Home");
        }


    }

}






