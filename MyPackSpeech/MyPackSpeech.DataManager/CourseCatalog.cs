using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using MyPackSpeech.DataManager.Data;
using Newtonsoft.Json.Linq;

namespace MyPackSpeech.DataManager
{
   public class CourseCatalog
   {
      public static CourseCatalog Instance;

      public List<Course> Courses { get; private set; }
      public List<Department> Departments { get; private set; }
      private const string departmentsList = "CourseData/prefixes.txt";

      public CourseCatalog()
      {
         Instance = this;

         Courses = new List<Course>();
         Departments = new List<Department>();
         LoadData();

      }

      public Department GetDepartment(String prefix)
      {
         return Departments.Find(d => d.Abv.Equals(prefix));
      }

      private void LoadData()
      {
         Debug.WriteLine("Reading list of departments");
         StreamReader deparmentsReader = new StreamReader(departmentsList);
         string line;
         while ((line = deparmentsReader.ReadLine()) != null)
         {
            string[] lineParts = line.Split("-".ToCharArray());
            if (lineParts.Length >= 2)
            {
               Department dept = new Department(line.Substring(line.IndexOf("-")).Trim(), lineParts[0].Trim());
               String filename = String.Format("CourseData/{0}.json", dept.Abv);

               if (File.Exists(filename))
               {
                  Departments.Add(dept);
                  Debug.WriteLine(dept);


                  String fileContents = System.IO.File.ReadAllText(filename);
                  JArray json = JArray.Parse(fileContents);
                  foreach (JToken courseObject in json)
                  {
                     //Debug.WriteLine(courseObject["name"]);
                     int courseNumber = int.Parse(courseObject["number"].ToString());
                     string description = "";
                     if (courseObject["description"] != null)
                     {
                        description = courseObject["description"].ToString();
                     }
                     string name = "";
                     if (courseObject["name"] != null)
                     {
                        name = courseObject["name"].ToString();
                     }
                     Course course = new Course(dept, name, courseNumber, description);
                     Courses.Add(course);
                  }
               }
            }
         }
      }
   }
}