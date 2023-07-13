using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Programming_Assessment.Models;

namespace Programming_Assessment.Controllers
{
    public class RegistrationController : Controller
    {
        private ProgrammingAssessmentEntities5 db = new ProgrammingAssessmentEntities5();

        // GET: Registration
        public ActionResult Index()
        {
            var registrations = db.Registrations.Include(r => r.Semester).Include(r => r.Course).Include(r => r.User);
            return View(registrations.ToList());
        }

        // GET: Registration/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: Registration/Create
        public ActionResult Create()
        {
            ViewBag.SemesterID = new SelectList(db.Semesters, "IDSemester", "SemesterName");
            ViewBag.CourseID = new SelectList(db.Courses, "IDCourse", "CourseName");

            var Users =
            db.Users.Select(s => new
            {
                IDUser = s.IDUser,
                Name = s.FIrstName + " " + s.LastName,
            }).ToList();

            ViewBag.UserID = new SelectList(Users, "IDUser", "Name");

            return View();
        }

        // POST: Registration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDRegistration,FirstName,LastName,CourseID,SemesterID,PhoneNumber,UserID")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                if (registration.UserID != null)
                {
                    var User =
                    db.Users
                    .Where(s => s.IDUser == registration.UserID)
                    .Select(s => new
                    {
                        LastName = s.LastName,
                        FIrstName = s.FIrstName,
                    }).FirstOrDefault();

                    registration.FirstName = User.FIrstName;
                    registration.LastName = User.LastName;
                }

                db.Registrations.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SemesterID = new SelectList(db.Semesters, "IDSemester", "SemesterName", registration.SemesterID);
            ViewBag.CourseID = new SelectList(db.Courses, "IDCourse", "CourseName", registration.CourseID);
            ViewBag.UserID = new SelectList(db.Users, "IDUser", "FirstName", registration.UserID);

            return View(registration);
        }

        // GET: Registration/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.SemesterID = new SelectList(db.Semesters, "IDSemester", "SemesterName", registration.SemesterID);
            ViewBag.CourseID = new SelectList(db.Courses, "IDCourse", "CourseName", registration.CourseID);

            var Users =
            db.Users
            .Select(s => new
            {
                IDUser = s.IDUser,
                Name = s.FIrstName + " " + s.LastName,
            }).ToList();

            ViewBag.UserID = new SelectList(Users, "IDUser", "Name", registration.UserID);
            return View(registration);
        }

        // POST: Registration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDRegistration,FirstName,LastName,CourseID,SemesterID,PhoneNumber,UserID")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                if (registration.UserID != null)
                {
                    var User =
                    db.Users
                    .Where(s => s.IDUser == registration.UserID)
                    .Select(s => new
                    {
                        LastName = s.LastName,
                        FIrstName = s.FIrstName,
                    }).FirstOrDefault();

                    registration.FirstName = User.FIrstName;
                    registration.LastName = User.LastName;
                }

                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SemesterID = new SelectList(db.Semesters, "IDSemester", "SemesterName", registration.SemesterID);
            ViewBag.CourseID = new SelectList(db.Courses, "IDCourse", "CourseName", registration.CourseID);
            ViewBag.UserID = new SelectList(db.Users, "IDUser", "FirstName", registration.UserID);
            return View(registration);
        }

        // GET: Registration/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Registration registration = db.Registrations.Find(id);
            db.Registrations.Remove(registration);
            db.SaveChanges();
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
