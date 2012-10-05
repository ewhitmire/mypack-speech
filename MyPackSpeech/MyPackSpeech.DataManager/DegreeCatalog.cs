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
      private Dictionary<String, CourseFilter> orphanedFilters;
      private const string degreeList = "Curricula/degrees.txt";

      public DegreeCatalog()
      {
         degrees = new List<DegreeProgram>();
         orphanedFilters = new Dictionary<String, CourseFilter>();
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

               foreach (JToken jsonItem in json)
               {
                  
                  if (jsonItem["type"].ToString().Equals("filter"))
                  {
                     CourseFilter filter = ParseCourseFilter(jsonItem["filter"]);
                     orphanedFilters.Add(jsonItem["id"].ToString(), filter);
                  }
                  else
                  {
                     // likely only 1 per file
                     String name = jsonItem["name"].ToString();
                     DegreeProgram program = new DegreeProgram(name);
                     degrees.Add(program);

                     JToken reqs = jsonItem["requirements"];

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
         else if (json["id"] != null)
         {
            c = orphanedFilters[json["id"].ToString()];
         }
         else if (json["department"] != null)
         {
            // Must be something like CSC 400+
            c = new CourseFilter();
            c.Dept = CourseCatalog.Instance.GetDepartment(json["department"].ToString());
            foreach (Operator op in Enum.GetValues(typeof(Operator)))
            {
               if (json[op.ToString()] != null)
               {
                  c.Op = op;
                  c.Number = int.Parse(json[op.ToString()].ToString());
               }
            }
         }
         else
         {
            c = new CourseFilter();
         }
         
         if (json["and"] != null)
         {
            foreach (JToken subfilter in json["and"])
            {
               c.And.Add(ParseCourseFilter(subfilter));
            }
         }
         
         if (json["or"] != null)
         {
            foreach (JToken subfilter in json["or"])
            {
               c.Or.Add(ParseCourseFilter(subfilter));
            }
         }

         if (json["not"] != null)
         {
            foreach (JToken subfilter in json["not"])
            {
               c.Not.Add(ParseCourseFilter(subfilter));
            }
         }

         return c;
      }
   }
}
