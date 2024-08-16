using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP5125_Cumulative_Project_Part_3.Models;

namespace HTTP5125_Cumulative_Project_Part_3.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }


        // GET: /Teacher/List
        public ActionResult List(string searchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(searchKey);

            return View(Teachers);
        }

        // GET: /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.DetailTeacherInfo(id);

            return View(NewTeacher);
        }

        // GET: /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.DetailTeacherInfo(id);
            return View(NewTeacher);
        }


        // POST: /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET: /Teacher/New
        public ActionResult New()
        {
            return View();
        }

        //POST: /Teacher/Create
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, decimal Salary)
        {

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.Salary = Salary;
            NewTeacher.EmployeeNumber = EmployeeNumber;

            TeacherDataController controller = new TeacherDataController();
            controller.CreateTeacher(NewTeacher);

            return RedirectToAction("List");
        }



        /// <summary>
        /// Routes to a dynamically generated "Teacher Update" page. Gathers information from database
        /// </summary>
        /// <param name="id">ID of the Teacher</param>
        /// <returns>A dynamic "update teacher" webpage which provides the current information of the teacher and ask the user for information as part of a form.</returns>
        /// <example>GET: /Teacher/Update/{id}</example>
        //GET: /Teacher/Update/{id}
        [HttpGet]
        public ActionResult Update(int id)
        {

            TeacherDataController controller = new TeacherDataController(); ;
            Teacher selectedTeacher = controller.DetailTeacherInfo(id);
            return View(selectedTeacher);
        }



        /// <summary>
        ///  Receives a POST request containing information about an existing teacher in the system with new values.
        /// </summary>
        /// <param name="id">ID of the Teacher to update (primary key)</param>
        /// <param name="TeacherFname">The updated first name of the teacher</param>
        /// <param name="TeacherLname">The updated last name of the teacher</param>
        /// <param name="EmployeeNumber">The updated employeenumber of the teacher</param>
        /// <param name="Salary">The updated salary of the teacher</param>
        /// <returns>A dynamic webpage with provides the current information of the teacher</returns>
        /// <example>POST: /Teacher/Updated/{id}</example>
        //POST: /Teacher/Updated/{id}
        [HttpPost]
        public ActionResult Updated(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, decimal Salary)
        {
            //Debug.WriteLine("working");


            Teacher TeacherInfo = new Teacher();
            TeacherInfo.TeacherFname = TeacherFname;
            TeacherInfo.TeacherLname = TeacherLname;
            TeacherInfo.EmployeeNumber = EmployeeNumber;
            TeacherInfo.Salary = Salary;



            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);

            return RedirectToAction("Show/" + id);
        }

        /// <summary>
        /// An AJAX UPDATE page which will send the HTTP request to the api controller and update teacher's info by using JAVASCRIPT
        /// </summary>
        /// <param name="id">ID of teacher (primary key)</param>
        /// <returns></returns>
        public ActionResult AJAX_Update(int id)
        {

            TeacherDataController controller = new TeacherDataController(); ;
            Teacher selectedTeacher = controller.DetailTeacherInfo(id);
            return View(selectedTeacher);
        }

        /// <summary>
        ///  Receives a POST request containing information about an existing teacher in the system with new values.
        /// </summary>
        /// <param name="id">ID of the Teacher to update (primary key)</param>
        /// <param name="TeacherFname">The updated first name of the teacher</param>
        /// <param name="TeacherLname">The updated last name of the teacher</param>
        /// <param name="EmployeeNumber">The updated employeenumber of the teacher</param>
        /// <param name="Salary">The updated salary of the teacher</param>
        /// <returns>A dynamic webpage with provides the current information of the teacher</returns>
        /// <example>POST: /Teacher/Updated/{id}</example>
        //POST: /Teacher/Updated/{id}
        [HttpPost]
        public ActionResult AJAX_Updated(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, decimal Salary)
        {
            //Debug.WriteLine("working");


            Teacher TeacherInfo = new Teacher();
            TeacherInfo.TeacherFname = TeacherFname;
            TeacherInfo.TeacherLname = TeacherLname;
            TeacherInfo.EmployeeNumber = EmployeeNumber;
            TeacherInfo.Salary = Salary;



            TeacherDataController controller = new TeacherDataController();
            controller.AJAX_UpdateTeacher(id, TeacherInfo);

            return RedirectToAction("Show/" + id);
        }


        /// <summary>
        /// An AJAX NEW page which will send the HTTP request to api controller and create a new teacher by using JAVASCRIPT
        /// </summary>
        /// <returns></returns>
        //GET: /Teacher/AJAX_New
        public ActionResult AJAX_New()
        {
            return View();
        }

    }
}