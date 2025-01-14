﻿using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();
        public SchoolContext DbContext
        {
            get { return db; }
            set { db = value; }
        }

        // GET: Student

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)//TODO : Remplacer les paramètres par un ViewModel
        {

            if (Session["UserID"] == null)
            {

                return RedirectToAction("Index", "Home");
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = from s in db.Students
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }


        // GET: Student/Details/5
        public ActionResult Details(StudentVM model)
        {
            if (Session["UserID"] == null)// Remplaçable par filtres prédéfinis d'authentification ou personnalisés
            {
                return RedirectToAction("Index", "Home");//TO DO: Factoriser pour éviter la redondance d'utilisation
            }
            if (model.ID == 0)
            {
                return View(model);
            }
            TempData["StudentID"] = model.ID;//TO DO: Remplacer par l'ID de la personne connecté via 'Session["UserID"]'

            Student student = db.Students.Find(model.ID);
            if (student == null)
            {
                return HttpNotFound();
            }
            //
            List<Course> listCourses = db.Courses.OrderBy(c => c.Title).ToList();
            ViewBag.listCourses = listCourses;

            model.EnrollmentDate = db.Students.Where(e => e.ID == model.ID).Select(e => e.EnrollmentDate).FirstOrDefault();
            model.FirstMidName = db.People.Where(e => e.ID == model.ID).Select(e => e.FirstMidName).FirstOrDefault();
            model.LastName = db.People.Where(e => e.ID == model.ID).Select(e => e.LastName).FirstOrDefault();
            model.ImagePath = db.People.Where(e => e.ID == model.ID).Select(e => e.ImagePath).FirstOrDefault(); ;
            model.Enrollments = db.Enrollments.Where(e => e.StudentID == model.ID).OrderBy(e => e.Course.Title).ToList();

            return View(model);

        }
        [HttpPost]
        public ActionResult Details(string listCourses)
        {
            if (Session["UserID"] == null)
            {

                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                int courseID = int.Parse(listCourses);
                int studentID = int.Parse(TempData["StudentID"].ToString());
                
                if (!(db.Enrollments.Where(o => o.Student.ID == studentID && o.CourseID == courseID).Any()))
                {
                    Enrollment nouveauCours = new Enrollment
                    {
                        CourseID = courseID,
                        StudentID = studentID,
                        
                    };
                    db.Enrollments.Add(nouveauCours);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = TempData["StudentID"] });
                }
                else
                {
                    TempData["ErrorMessage"] = "You're already subscribed to this lesson";
                    return RedirectToAction("Details", new { id = TempData["StudentID"] });
                }
            }

            else
            {
                Student student = db.Students.Find(TempData["StudentID"]);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
        }



        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName, FirstMidName, EnrollmentDate")]Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }


        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] == null)
            {

                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (Session["UserID"] == null)
            {

                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = db.Students.Find(id);
            if (TryUpdateModel(studentToUpdate, "",
               new string[] { "LastName", "FirstMidName", "EnrollmentDate" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        { if (Session["UserID"] == null)
            {

                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
