using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using HTTP5125_Cumulative_Project_Part_3.Models;


namespace HTTP5125_Cumulative_Project_Part_3.Controllers
{
    public class ClassDataController : ApiController
    {
        //The database context class which allows us to access our MySQL Database
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// returns a list of classes in database
        /// </summary>
        /// <example>
        /// GET: api/ClassData/ListClass
        /// </example>
        /// <returns>A list of class(calssCode, classId, className)</returns>

        [HttpGet]
        [Route("api/ClassData/ListClass/{searchKey?}")]
        public IEnumerable<Class> ListClass(string searchKey = null)
        {
            //create the instance connection to school database
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //create a new command
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL query
            cmd.CommandText = "Select * from classes where lower(classcode) like lower(@key) ";

            cmd.Parameters.AddWithValue("key", "%" + searchKey + "%");
            cmd.Prepare();

            //gather result set of query into a variable
            MySqlDataReader resultSet = cmd.ExecuteReader();

            //create an empty list of classes
            List<Class> classes = new List<Class> { };

            //loop through each row in the result set
            while (resultSet.Read())
            {
                //access column information by their database column name as an index and convert to int/string
                int ClassId = Convert.ToInt32(resultSet["classid"]);
                string ClassCode = resultSet["classcode"].ToString();
                string ClassName = resultSet["classname"].ToString();




                //create a NewClass object which is the new instantiation of the Classes class
                Class newClass = new Class();

                newClass.ClassCode = ClassCode;
                newClass.ClassId = ClassId;
                newClass.ClassName = ClassName;


                classes.Add(newClass);
            }

            //close connection between webserver and database
            Conn.Close();


            return classes;

        }


        /// <summary>
        /// Find the class in the database with an id
        /// </summary>
        /// <param name="id">the class primary key(classid)</param>
        /// <example>
        /// GET: api/ClassData/ClassDetails/{id}     -> A Class object
        /// </example>
        /// <example>
        /// GET: api/ClassData/ClassDetails/2     ->
        /// 
        ///<Class>
        ///<ClassCode>http5102</ClassCode>
        ///<ClassId>2</ClassId>
        ///<ClassName>Project Management</ClassName>
        ///<FinishDate>2018-12-14 12:00:00 AM</FinishDate>
        ///<StartDate>2018-09-04 12:00:00 AM</StartDate>
        ///<TeacherId>2</TeacherId>
        ///</Classes>
        /// 
        /// </example>
        /// <returns>A Classes object</returns>
        [HttpGet]
        public Class ClassDetails(int id)
        {
            //create the instance connection to school database
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //create a new command
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL query
            cmd.CommandText = "Select * from classes left join teachers on classes.teacherid = teachers.teacherid where classid =@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //gather result set of query into a variable
            MySqlDataReader resultSet = cmd.ExecuteReader();

            //create a NewClass object which is the new instantiation of the Classes class
            Class newClass = new Class();

            //loop through each row in the result set
            while (resultSet.Read())
            {
                //access column information by their database column name as an index and convert to int/string
                int ClassId = (int)resultSet["classid"];
                string ClassCode = resultSet["classcode"].ToString();
                int TeacherId = Convert.ToInt32(resultSet["teacherid"]);
                string StartDate = resultSet["startdate"].ToString();
                string FinishDate = resultSet["finishdate"].ToString();
                string ClassName = resultSet["classname"].ToString();

                //access information
                string teacherfname = resultSet["teacherfname"].ToString();
                string teacherlname = resultSet["teacherlname"].ToString();


                newClass.ClassCode = ClassCode;
                newClass.ClassId = ClassId;
                newClass.TeacherId = TeacherId;
                newClass.ClassName = ClassName;
                newClass.StartDate = StartDate;
                newClass.FinishDate = FinishDate;
                newClass.TeacherFname = teacherfname;
                newClass.TeacherLname = teacherlname;

            }

            //close connection between webserver and database
            Conn.Close();


            return newClass;
        }


        /// <summary>
        /// Return a list of classes which are teached by selected teacher
        /// </summary>
        /// <param name="teacherId">ID of the selected teacher (primary key)</param>
        /// <example>
        /// GET: api/ClassData/ListClassForTeacher/{teacherId} -> List of classes
        /// </example>
        /// <example>
        /// GET: api/ClassData/ListClassForTeacher/5 ->
        /// <CLASS>
        ///     <ClassCode>http5126</ClassCode>
        ///     <ClassId>3</ClassId>
        ///     <ClassName>Database design</ClassName>
        ///     <TeacherFname>Jemi</TeacherFname>
        ///     <TeacherLname>Choi</TeacherLname>
        /// </CLASS>
        /// <CLASS>
        ///     <ClassCode>http5114</ClassCode>
        ///     <ClassId>9</ClassId>
        ///     <ClassName>Workshop in webdev</ClassName>
        ///     <TeacherFname>Gourav</TeacherFname>
        ///     <TeacherLname>Bhanot</TeacherLname>
        /// </CLASS>
        /// </example>
        /// <returns>A list of classes</returns>
        [HttpGet]
        [Route("api/ClassData/ListClassForTeacher/{teacherId}")]
        public IEnumerable<Class> ListClassForTeacher(int teacherId)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL COMMAND
            cmd.CommandText = "select * from classes left join teachers on classes.teacherid = teachers.teacherid where classes.teacherid = @id";
            cmd.Parameters.AddWithValue("@id", teacherId);
            cmd.Prepare();
            MySqlDataReader resultSet = cmd.ExecuteReader();

            List<Class> classes = new List<Class>();

            while (resultSet.Read())
            {

                string ClassCode = resultSet["classcode"].ToString();
                string ClassName = resultSet["classname"].ToString();
                int ClassId = Convert.ToInt32(resultSet["classid"]);
                string teacherFname = resultSet["teacherfname"].ToString();
                string teacherLname = resultSet["teacherlname"].ToString();

                Class selectedClass = new Class();

                selectedClass.ClassCode = ClassCode;
                selectedClass.ClassName = ClassName;
                selectedClass.ClassId = ClassId;
                selectedClass.TeacherFname = teacherFname;
                selectedClass.TeacherLname = teacherLname;


                classes.Add(selectedClass);

            }

            Conn.Close();

            return classes;


        }



    }
}