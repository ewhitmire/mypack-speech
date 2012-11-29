using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using MyPackSpeech.DataManager.Data;
using Newtonsoft.Json.Linq;
using MyPackSpeech.DataManager.Data.Filter;

namespace MyPackSpeech.DataManager
{
   public class DegreeCatalog
   {
      private static DegreeCatalog instance = null;
      public static DegreeCatalog Instance
      {
         get
         {
            if (instance == null)
               instance = new DegreeCatalog();
            return instance;
         }
      }

      public List<DegreeProgram> Degrees;
      private Dictionary<String, IFilter<Course>> orphanedFilters;
      private const string degreeList = "Curricula/degrees.txt";

      private DegreeCatalog()
      {
         Degrees = new List<DegreeProgram>();
         orphanedFilters = new Dictionary<string, IFilter<Course>>();
         LoadData();
      }
      public void LoadData()
      {
         Debug.WriteLine("Reading list of degrees");
         try
         {
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
                        IFilter<Course> filter = ParseCourseFilter(jsonItem["filter"]);
                        if (!orphanedFilters.ContainsKey(jsonItem.Value<String>("id")))
                        {
                           orphanedFilters.Add(jsonItem.Value<String>("id"), filter);
                        }
                     }
                     else
                     {
                        // likely only 1 per file
                        String name = jsonItem["name"].ToString();
                        DegreeProgram program = new DegreeProgram(name);
                        Degrees.Add(program);

                        JToken reqs = jsonItem["requirements"];

                        foreach (JToken jsonDegreeCategory in reqs)
                        {
                           DegreeRequirementCategory cat = new DegreeRequirementCategory(jsonDegreeCategory["name"].ToString());
                           JToken filters = jsonDegreeCategory["filters"];

                           foreach (JToken jsonCourseFilter in filters)
                           {
                              IFilter<Course> courseFilter = ParseCourseFilter(jsonCourseFilter);
                              if (courseFilter != null)
                              {
                                 DegreeRequirement degreeReq = new DegreeRequirement();
                                 degreeReq.Category = cat;
                                 degreeReq.CourseRequirement = courseFilter;
                                 if (jsonCourseFilter["id"] != null)
                                 {
                                    degreeReq.Name = jsonCourseFilter.Value<String>("id");
                                 }
                                 program.Requirements.Add(degreeReq);
                              }
                           }
                        }
                     }
                  }
               }
            }
         }
         catch (Exception e)
         {
            Debug.WriteLine("Count not read degrees.txt");
            Debug.WriteLine(e.Message);
         }
      }

      public IFilter<Course> ParseCourseFilter(JToken json)
      {
         IFilter<Course> c = null;
         if (json["course"] != null)
         {
            String courseName = json["course"].ToString();
            String[] data = courseName.Split(' ');

            Department dept = CourseCatalog.Instance.GetDepartment(data[0]);
            if (dept != null)
            {
               int number = int.Parse(data[1]);
               c = CourseFilter.DeptName(dept.Name).And(CourseFilter.Number(number));
            }
         }
         else if (json["id"] != null)
         {
            String key = json.Value<String>("id");
            c = orphanedFilters[key];
         }
         else if (json["department"] != null)
         {
            // Must be something like CSC 400+
            Department dept = CourseCatalog.Instance.GetDepartment(json["department"].ToString());
            if (null != dept)
            {
               c = CourseFilter.DeptName(dept.Name);

               IFilter<Course> numbers = null;
               foreach (Operator op in Enum.GetValues(typeof(Operator)))
               {
                  var val = json[op.ToString()];
                  if (val != null)
                  {
                     int num = int.Parse(val.ToString());
                     if (json[op.ToString()] != null)
                     {
                        if (numbers == null)
                           numbers = CourseFilter.Number(num, op);
                        else
                           numbers = numbers.Or(CourseFilter.Number(num, op));
                     }
                  }
               }
               if (numbers != null)
               {
                  c = c.And(numbers);
               }
            }
         }

         if (json["and"] != null)
         {
            foreach (JToken subfilter in json["and"])
            {
               var filter = ParseCourseFilter(subfilter);
               if (c == null)
                  c = filter;
               else
                  c = c.And(filter);
            }
         }

         if (json["or"] != null)
         {
            foreach (JToken subfilter in json["or"])
            {
               var filter = ParseCourseFilter(subfilter);
               if (c == null)
                  c = filter;
               else
                  c = c.Or(filter);
            }
         }

         if (json["not"] != null)
         {
            foreach (JToken subfilter in json["not"])
            {
               var filter = ParseCourseFilter(subfilter).Not();
               if (c == null)
                  c = filter;
               else
                  c = c.And(filter);
            }
         }

         return c;
      }
   }
}