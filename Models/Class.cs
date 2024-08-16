using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP5125_Cumulative_Project_Part_3.Models
{
    public class Class
    {
        //the following fileds define a class
        public string ClassCode;
        public int TeacherId;
        public string ClassName;
        public string StartDate;
        public string FinishDate;
        public int ClassId;


        //add more details of teacher to link class with teacher
        public string TeacherFname;
        public string TeacherLname;
    }
}