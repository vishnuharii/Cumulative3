using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using HTTP5125_Cumulative_Project_Part_3.Models;

namespace HTTP5125_Cumulative_Project_Part_3.Controllers
{
    public class TeacherDataController : ApiController
    {
        //The database context class which allows us to access our MySQL Database

        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Return a list of Teacher in the database
        /// </summary>
        /// <example>
        /// GET: api/TeacherData/ListTeachers
        /// </example>
        /// <returns>A list of teachers(firstnames and lastnames)</returns>

        [HttpGet]
        [Route("api/TeacherData/ListTeacher/{searchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string searchKey = null)
        {
            //create the instance connection to school database
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //create a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL query
            cmd.CommandText = "select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ' , teacherlname)) like lower(@key)";


            cmd.Parameters.AddWithValue("key", "%" + searchKey + "%");
            cmd.Prepare();

            //gather result set of query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //create an empty list of teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //loop through each row in the result set
            while (ResultSet.Read())
            {
                //access column information by their database column name as an index and convert to int/string
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];

                //create a NewTeacher object which is the new instantiation of the Teacher class
                Teacher NewTeacher = new Teacher();

                //set the NewTeacher's property (as NewTeacher is an object)
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;

                //add the teacher name to the list
                Teachers.Add(NewTeacher);

            }

            //close the connection between webserver and database
            Conn.Close();

            //return the list of teachers' names
            return Teachers;

        }



        /// <summary>
        /// Finds a teacher in the system given an ID
        /// </summary>
        /// <param name="id">The teacher primary key</param>
        /// <example>
        /// GET: api/teacherdata/DetailTeacherInfo/{id}   -> A teacher object
        /// </example>
        /// <example>
        /// GET: api/teacherdata/DetailTeacherInfo/3     ->
        /// 
        ///<Teacher>
        ///<EmployeeNumber>T382</EmployeeNumber>
        ///<Salary>60.22</Salary>
        ///<TeacherFname>Linda</TeacherFname>
        ///<TeacherId>3</TeacherId>
        ///<TeacherLname>Chan</TeacherLname>
        ///</Teacher>
        ///
        /// </example>
        /// <returns>A teacher object</returns>
        [HttpGet]
        public Teacher DetailTeacherInfo(int id)
        {
            //create a NewTeacher object which is the new instantiation of the Teacher class
            Teacher NewTeacher = new Teacher();

            //create the instance connection to school database
            MySqlConnection Conn = School.AccessDatabase();

            //open the connection between webserver and database
            Conn.Open();

            //create a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL query
            cmd.CommandText = "Select * from teachers where teacherid = " + id;

            //gather result set of query into a variable ResultSet
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //loop the results
            while (ResultSet.Read())
            {
                //access the column information by database column name as an index and convert them to int/string/decimal
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                decimal Salary = (decimal)ResultSet["salary"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];


                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.Salary = Salary;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
            }

            //close the connection
            Conn.Close();

            //return the object NewTeacher
            return NewTeacher;

        }



        /// <summary>
        /// Delete a Teacher from database
        /// </summary>
        /// <param name="id">The teacher primary key</param>
        /// <example>POST: api/teacherdata/DeleteTeacher/3</example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //open connection
            Conn.Open();

            //Establish a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //close the connection
            Conn.Close();

        }


        /// <summary>
        /// Create a new Teacher into database
        /// </summary>
        /// <param name="NewTeacher">contains all the parameters such as teacherFname, teacherLname, EmployeeNumber, Salary</param>
        /// <example>curl -H "Content-Type:application/json" -d @teacherdata.json "http://localhost:61041/api/TeacherData/CreateTeacher/10"
        /// {
        ///     "TeacherFname":"Jhon",
        ///     "TeacherLname":"Adams",
        ///     "EmployeeNumber":"781",
        ///     "Salary":"80"
        /// }
        /// </example>
        [HttpPost]
        public void CreateTeacher([FromBody] Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //open connectio
            Conn.Open();

            //Establish a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL COMMAND
            cmd.CommandText = "Insert into teachers(teacherfname, teacherlname, employeenumber, hiredate, salary) VALUES(@teacherfname, @teacherlname, @employeenumber, CURRENT_DATE(), @salary) ";
            cmd.Parameters.AddWithValue("@teacherfname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@teacherlname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Close connection
            Conn.Close();

        }

        /// <summary>
        /// Update a teacher's info into database
        /// </summary>
        /// <param name="id">ID of a teacher (primary key)</param>
        /// <param name="SelectedTeacher">contains all the parameters such as teacherFname, teacherLname, EmployeeNumber, Salary</param>
        /// <example>using CURL request with a JSON object to update:
        /// notepad teacherdata.json
        /// {
        ///     "TeacherFname":"Jhon",
        ///     "TeacherLname":"Adams",
        ///     "EmployeeNumber":"187",
        ///     "Salary":"80"
        /// }
        /// curl -H "Content-Type:application/json" -d @teacherdata.json "http://localhost:61041/api/TeacherData/UpdateTeacher/17"
        /// </example>
        [HttpPost]
        public void UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //open connection
            Conn.Open();

            //Establish a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL COMMAND
            cmd.CommandText = "Update teachers set teacherfname=@teacherfname, teacherlname=@teacherlname, employeenumber=@employeenumber, salary=@salary where teacherid=@teacherId";
            cmd.Parameters.AddWithValue("@teacherfname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@teacherlname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@teacherId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Close connection
            Conn.Close();


        }

        /// <summary>
        /// Update a teacher's info into database
        /// </summary>
        /// <param name="id">ID of a teacher (primary key)</param>
        /// <param name="SelectedTeacher">contains all the parameters such as teacherFname, teacherLname, EmployeeNumber, Salary</param>
        /// <example>using CURL request with a JSON object to update:
        /// notepad teacherdata.json
        /// {
        ///     "TeacherFname":"Jhon",
        ///     "TeacherLname":"Adams",
        ///     "EmployeeNumber":"781",
        ///     "Salary":"80"
        /// }
        /// curl -H "Content-Type:application/json" -d @teacherdata.json "http://localhost:61041/api/TeacherData/Ajax_UpdateTeacher/17"
        /// </example>
        [HttpPost]
        public void AJAX_UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //open connection
            Conn.Open();

            //Establish a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL COMMAND
            cmd.CommandText = "AJAX_Update teachers set teacherfname=@teacherfname, teacherlname=@teacherlname, employeenumber=@employeenumber, salary=@salary where teacherid=@teacherId";
            cmd.Parameters.AddWithValue("@teacherfname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@teacherlname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@teacherId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Close connection
            Conn.Close();


        }








    }
}