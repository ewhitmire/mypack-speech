using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using MyPackSpeech.DataManager.Data;
using Newtonsoft.Json.Linq;

namespace MyPackSpeech.DataManager
{
   public class DegreeCatalog
   {
      public List<DegreeProgram> degrees;
      private const string degreeList = "Curricula/degrees.txt";

      public DegreeCatalog()
      {
         degrees = new List<DegreeProgram>();
         LoadData();
      }
      public void LoadData()
      {
         Debug.WriteLine("Reading list of degrees");
         StreamReader deparmentsReader = new StreamReader(degreeList);
         string degree;
         while ((degree = deparmentsReader.ReadLine()) != null)
         {
           
            String filename = String.Format("Curricula/{0}.json", degree);
            
            if (File.Exists(filename))
            {
               Debug.WriteLine(filename);

               String fileContents = System.IO.File.ReadAllText(filename);
               JArray json = JArray.Parse(fileContents);

               foreach (JToken jsonDegreeProgram in json)
               {
                  // likely only 1 per file
                  String name = jsonDegreeProgram["name"].ToString();
                  DegreeProgram program = new DegreeProgram(name);
                  degrees.Add(program);

                  JToken reqs = jsonDegreeProgram["requirements"];

                  foreach (JToken jsonDegreeCategory in reqs)
                  {
                     DegreeRequirementCategory cat = new DegreeRequirementCategory(jsonDegreeCategory["name"].ToString());
                     JToken filters = jsonDegreeCategory["filters"];

                     foreach (JToken jsonCourseFilter in filters)
                     {
                        CourseFilter courseFilter = ParseCourseFilter(jsonCourseFilter);
                        DegreeRequirement degreeReq = new DegreeRequirement();
                        degreeReq.CourseRequirement = courseFilter;

                        program.requirements.Add(degreeReq);
                     }
                  }
               }
            }
         }
      }

      public CourseFilter ParseCourseFilter(JToken json)
      {
         CourseFilter c;
         if (json["course"] != null)
         {
            String courseName = json["course"].ToString();
            String[] data = courseName.Split(' ');


            c = new CourseFilter();
            c.Op = Operator.EQ;
            c.Dept = CourseCatalog.Instance.GetDepartment(data[0]);
            c.Number = int.Parse(data[1]);
         }
         else
         {
            c = new CourseFilter();
         }
         return c;
      }
   }
}
