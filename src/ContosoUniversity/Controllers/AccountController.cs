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

            return View();
        }


        [HttpPost]
        public ActionResult Register(PersonVM model)
        {
            //FileInfo fileInfo = new FileInfo(model.ImageFile.FileName);
            if (model.Password == model.ConfirmPassword)
            {
                bool boolImage = false;
                string fileName = "";
                string extension = "";

                if (model.ImageFile != null)
                {
                    boolImage = true;
                    fileName = model.FirstMidName + model.LastName.ToUpper();
                    extension = Path.GetExtension(model.ImageFile.FileName);

                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    {
                        TempData["ImageMessage"] = "Authorized extension are JPG, JPEG and PNG";
                        return View(model);
                    }
                    else if (model.ImageFile.ContentLength > 100000)
                    {
                        TempData["ImageMessage"] = "Maximum size is 100 KB";
                        return View(model);
                    }
                    else
                    {
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        model.ImagePath = "/Image/" + fileName;
                        fileName = Path.Combine(Server.MapPath("/Image/"), fileName);

                    }
                }

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
                        if (boolImage == true)
                        {
                            model.ImageFile.SaveAs(fileName);
                        }
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
                            if (boolImage == true)
                            {
                                model.ImageFile.SaveAs(fileName);
                            }
                            ViewBag.Message = "Welcome " + user.Username;
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        TempData["UsernameMessage"] = "Username already exists";
                        return View();
                    }
                }
            }
            TempData["PasswordMessage"] = "Password Not Conform";
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
                else { TempData["ErrorLoginMessage"] = "Username or Password is wrong"; }



            }
            return View();

        }

        public ActionResult Logout()
        {
            if (Session["UserID"] != null)
            {

                FormsAuthentication.SignOut();
            }

            //    FormsAuthentication.SignOut();
            //}
            Session["UserID"] = null;

            return RedirectToAction("Index", "Home");
        }


    }

}






