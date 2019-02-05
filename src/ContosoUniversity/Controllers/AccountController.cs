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
            using (SchoolContext db = new SchoolContext())
            {
                return View(db.userAccount.ToList());
            }

        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (SchoolContext db = new SchoolContext())
                {
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + "successfully registered";
            }
            return RedirectToAction("Index");
        }
        //Login
        public ActionResult Login()
        {
            return View();

        }
        //public ActionResult Login()
        //{
        //    Student student = new Student();
        //    Session["ID"] = student.ID.ToString();
        //    return View("Login");
        //}
        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            using (SchoolContext db = new SchoolContext())
            {
                var usr = db.userAccount.Single(u => u.Username == user.Username && u.Password == user.Password);
                if (usr != null)
                {
                    Session["UserID"] = usr.UserID.ToString();
                    Session["Username"] = usr.Username.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is wrong");
                }

            }
            return View();

        }
        public ActionResult LoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["LogoutMessage"] = "You are now successful loged out.";
                FormsAuthentication.SignOut();
            }


            return View("Index");
        }


    }

}



        

    
