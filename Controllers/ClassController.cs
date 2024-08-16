using HTTP5125_Cumulative_Project_Part_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace HTTP5125_Cumulative_Project_Part_3.Controllers
{
    public class ClassController : Controller
    {

        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        //Get: Class/List
        public ActionResult List(string searchKey = null)
        {

            ClassDataController controller = new ClassDataController();
            IEnumerable<Class> Classes = controller.ListClass(searchKey);

            return View(Classes);
        }

        //Get: Class/Show/{id}
        public ActionResult Show(int id)
        {
            ClassDataController controller = new ClassDataController();
            Class classes = controller.ClassDetails(id);

            return View(classes);
        }

        //get: Class/TeachingClass/{teacherId}
        public ActionResult TeachingClasses(int id)
        {
            ClassDataController controller = new ClassDataController();
            IEnumerable<Class> teachingClass = controller.ListClassForTeacher(id);

            return View(teachingClass);
        }


    }
}