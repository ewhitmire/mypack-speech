using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using MyPackSpeech.DataManager.Data;
using Newtonsoft.Json.Linq;
using MyPackSpeech.DataManager.Data.Filter;
using System.Collections.ObjectModel;

namespace MyPackSpeech.DataManager
{
   public class CourseCatalog
   {
      private const string DepartmentsList = "CourseData/prefixes.txt";

      event EventHandler filterChanged = null;
      private IFilter<Course> filter;

      private ObservableCollection<Course> filteredCourses = null;
      private void clearFilteredCourses()
      {
         if (filteredCourses != null)
            filteredCourses.Clear();
         filteredCourses = null;
      }
      private static CourseCatalog instance = null;
      public static CourseCatalog Instance
      {
         get
         {
            if (instance == null)
               instance = new CourseCatalog();
            return instance;
         }
      }
      public IEnumerable<Course> GetCourses(IFilter<Course> courseFilter)
      {
         return Courses.Where<Course>(c => courseFilter.Matches(c));
      }
      public List<Course> Courses { get; private set; }
      public List<Department> Departments { get; private set; }
      public event EventHandler FilterChanged { add { filterChanged += value; } remove { filterChanged -= value; } }
      public IFilter<Course> Filter
      {
         get { return filter; }
         set
         {
            filter = value;
            OnFilterChanged();
         }
      }
      public ObservableCollection<Course> FilteredCourses
      {
         get
         {
            if (filteredCourses == null)
            {
               IFilter<Course> courseFilter = filter;
               if (courseFilter == null)
                  filteredCourses = new ObservableCollection<Course>(Courses.Take(200));
               else
                  filteredCourses = new ObservableCollection<Course>(Courses.Where(c => courseFilter.Matches(c)).Take(32));
            }
            return filteredCourses;
         }
      }
      protected CourseCatalog()
      {
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
         using (StreamReader deparmentsReader = new StreamReader(DepartmentsList))
         {
            string line;
            while ((line = deparmentsReader.ReadLine()) != null)
            {
               string[] lineParts = line.Split("-".ToCharArray());
               if (lineParts.Length >= 2)
               {
                  string dname = line.Substring(line.IndexOf("-") + 1).Trim();
                  Department dept = new Department(dname, lineParts[0].Trim());
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
      private void OnFilterChanged()
      {
         clearFilteredCourses();
         EventHandler evt = filterChanged;
         if (evt != null)
            evt(this, EventArgs.Empty);

      }
   }
}