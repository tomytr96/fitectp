using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    public class AuthentificationController : Controller
    {
        // GET: Authentification
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult RegisterPOST()
        {
            RedirectToAction View();
        }
    }
}