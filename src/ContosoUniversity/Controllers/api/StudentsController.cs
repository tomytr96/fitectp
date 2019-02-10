using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers.api
{
    public class StudentsController : ApiController
    {
        private SchoolContext db = new SchoolContext();

        // GET: api/Students/5

            //                              APIVM
        public IHttpActionResult GetStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            // TODO : Passer une liste d'objets et non une chaine de caractères

            //APIViewModel api = new APIViewModel(){
            //id=student.ID,
            //lastname= student.Lastname,
            //firstname = student.FirstMidName,
            //enrollementDate = student.EnrollmentDate,
            //enrollements = courseIDVM.CoursID
            //}

            Dictionary<string, object>DetailsStudent = new Dictionary<string, object>();

            List<string>CoursIDList= new List<string>();

            DetailsStudent.Add("id", student.ID);
            DetailsStudent.Add("lastname", student.LastName);
            DetailsStudent.Add("firstname",student.FirstMidName);
            DetailsStudent.Add("enrollementDate", student.EnrollmentDate);
            DetailsStudent.Add("enrollements",CoursIDList);

            foreach ( var item in student.Enrollments)
            {
                //TODO : Ne pas mettre les ':' car le serializer s'en onccupera si l'on a bien une liste d'objet
                CoursIDList.Add("CoursID : "+item.CourseID.ToString()); 
            }

            return Ok(DetailsStudent);
                      
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.People.Count(e => e.ID == id) > 0;
        }
    }
}