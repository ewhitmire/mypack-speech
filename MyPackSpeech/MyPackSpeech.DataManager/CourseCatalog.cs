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
         return Courses.AsParallel().Where<Course>(c => courseFilter.Matches(c));
      }
      public Course GetCourse(Department dept, int number)
      {
         IEnumerable<Course> courses = Courses.Where<Course>(c => c.Dept.Equals(dept) && c.Number.Equals(number));
         return courses.FirstOrDefault();
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

      private void WriteKeyWordFile()
      {
         if (File.Exists(Course.KeyWordFile))
            File.Delete(Course.KeyWordFile);
         Dictionary<string, int> words = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
         foreach (var course in Courses)
         {
            foreach (var word in course.KeyWords)
            {
               if (!words.ContainsKey(word))
                  words[word] = 0;
               words[word]++;
            }
         }
         using (StreamWriter sw = new StreamWriter(File.OpenWrite(Course.KeyWordFile)))
         {
            foreach (string word in words.OrderByDescending(kp => kp.Value).Select(kp => kp.Key))
            {
               int num;
               if (!int.TryParse(word, out num))
               {
                  sw.WriteLine(String.Format("{0},{1}", word, words[word]));
               }
            }
         }
      }
      public Department GetDepartment(String prefix)
      {
         return Departments.Find(d => d.Abv.Equals(prefix));
      }

      public Boolean isClassName(string myString) {
         Char[] chars = myString.ToCharArray();

         for (int i = 0; i < chars.Length; i++)
         {
            Char c = chars[i];
            if (!Char.IsUpper(c) && !Char.IsDigit(c))
            {
               return false;
            }
         }
         return true;
      }

      public Boolean isDept(string myString)
      {
         for (int i = 0; i < Departments.Count; i++) { 
            if(Departments[i].Abv.Equals(myString))
            {
               return true;
            }
         }
         return false;
      }

      public Boolean isClassNumber(string myString)
      {
         Char[] chars = myString.ToCharArray();
         if (chars.Length != 3)
            return false;

         for (int i = 0; i < chars.Length; i++)
         {
            Char c = chars[i];
            if (!Char.IsDigit(c))
            {
               return false;
            }
         }
         return true;
      }

      public Boolean isUpperCase(string myString)
      {
         Char[] chars = myString.ToCharArray();
         
         for (int i = 0; i < chars.Length; i++)
         {
            Char c = chars[i];
            if (!Char.IsUpper(c))
            {
               return false;
            }
         }
         return true;
      }


      public List<string> getPreReqs(string[] reqString)
      {
         // two changes that were manually made to CSC.json
         // CSC 112:
         // 'E115' becomes 'E 115'
         // CSC 512:
         // 'CSC 314 and 333' becomes 'CSC 314 and CSC 333'


         //Splits to a list
         List<string> tokens = reqString.ToList();

         //The final strings of class requirements
         List<string> classes = new List<string>();

         //Get rid of empty tokens
          while(tokens.Contains("")){
             tokens.Remove("");
          }


         // Filter out language
         for (int i = 0; i < tokens.Count; i++) 
         {
            string myString = tokens[i];
            // CSC, 591, CSC591
            if(isClassName(myString)){ 
               classes.Add(myString);
            }
               // and must have dept before and after
            else if (myString.Equals("and") && isClassName(tokens[i + 1])
               && isClassName(tokens[i - 1]))
            {
               classes.Add(myString);
            }
               // or must have dept before and after
            else if (myString.Equals("or") && isClassName(tokens[i + 1]) 
               && isClassName(tokens[i - 1])) 
            {
               classes.Add(myString);
            }
         }


         // IF crosslisted CSC/ECE choose CSC
         for (int i = 1; i < classes.Count - 1; i++) { 
            if(classes[i-1].Equals("CSC") && classes[i].Equals("ECE")){
               classes.RemoveAt(i);
            }
         }


         // remove hanging Depts
         for (int i = classes.Count - 2; i > -1; i--)
         {
            //Remove all GPA nonsense
            if (classes[i].Equals("GPA") || classes[i].Equals("2") || classes[i].Equals("75") || classes[i].Equals("7"))
            {
               classes.RemoveAt(i);
            }
            //Comes from cross listed classes, or majors.
            else if(isUpperCase(classes[i]) && !isClassNumber(classes[i+1])){
               classes.RemoveAt(i);
            } 

         }

         //Given the previos removals, is there an and/or at the beginning
         if (classes.Count != 0) { 
            if(classes[0].Equals("and") || classes[0].Equals("or")){
               classes.RemoveAt(0);
            }
         }

         //Given the previous removals is there something hanging at the end
         if (classes.Count != 0)
         {
            if (!isClassNumber(classes[classes.Count - 1]))
            {
               classes.RemoveAt(classes.Count - 1);
            }
         }

         //Given the previous removals, is there an and/or at the end
         if (classes.Count != 0) {
            if (classes[classes.Count - 1].Equals("and") || classes[classes.Count - 1].Equals("or"))
            {
               classes.RemoveAt(classes.Count - 1);
            }
         
         
         }


         try
         {
            for (int i = 0; i < classes.Count; i++) {
               if (isDept(classes[i])) {
                  filter = CourseFilter.DeptAbv(classes[i]);
               }
               
            }
         }
         catch (Exception e) {
            System.Console.WriteLine(e);
         }


         return classes;
      }

      private void LoadData()
      {
         int duplicates = 0;
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
                     //Debug.WriteLine(dept);


                     String fileContents = System.IO.File.ReadAllText(filename);
                     JArray json = JArray.Parse(fileContents);
                     foreach (JToken courseObject in json)
                     {
                        //Debug.WriteLine(courseObject["name"]);
                        int courseNumber = int.Parse(courseObject["number"].ToString());
                        string description = "";
                        string preReqs = "";
                        if (courseObject["description"] != null)
                        {
                           description = courseObject["description"].ToString();
                        }
                        string name = "";
                        if (courseObject["name"] != null)
                        {
                           name = courseObject["name"].ToString();
                        }

                        List<string> parsedPreReqs = null;
                        if (courseObject["prerequisites"] != null)
                        {
                           preReqs = courseObject["prerequisites"].ToString();
                           //System.Console.WriteLine("preReqs: " + preReqs);
                           if(dept.Abv.Equals("CSC")){
                              //System.Console.WriteLine("CSC" + courseNumber);
                              Char[] delims = { ' ', '(', ')', ':', ';','.', ',', '/' };
                              parsedPreReqs = getPreReqs(preReqs.Split(delims));
                           }
                        }

                        Boolean spring = true;
                        if (courseObject["spring"] != null)
                        {
                           string offeredInSpring = courseObject["spring"].ToString().ToLower();
                           spring = offeredInSpring.Equals("true");
                           //if (dept.Abv.Equals("CSC"))
                              //System.Console.WriteLine(offeredInSpring + ":Spring:" + dept.Abv + courseNumber);

                        }
                        Boolean fall = true;
                        if (courseObject["fall"] != null)
                        {
                           string offeredInfall = courseObject["fall"].ToString().ToLower();
                           fall = offeredInfall.Equals("true");
                           //if (!fall)
                           //   System.Console.WriteLine(offeredInfall + ":Fall: " + dept.Abv + courseNumber);
                        }

                        int credits = 0;
                        if (courseObject["units"] != null)
                        {
                           int.TryParse(courseObject.Value<String>("units"), out credits);
                        }

                        if (dept.Abv == "CSC" && courseNumber == 246)
                        {
                           int foo = 0;
                           foo++;
                        }
                        IFilter<Course>[] prereqFilters = PrereqBuilder.GetPreReqFilters(parsedPreReqs);
                        Course course = new Course(dept, name, courseNumber, description, credits, prereqFilters);
                        course.spring = spring;
                        course.fall = fall;


                        if (!Courses.Contains(course))
                        {
                           Courses.Add(course);
                        }
                        else
                        {
                           duplicates++;
                        }
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

      public static String FormatCourseList(IEnumerable<Course> courseList)
      {
         List<Course> courses = courseList.ToList();
         
         if (courses.Count <= 3)
         {
            return String.Join(", ", courses.Select(c => c.ToString()));
         }
         else
         {
            return String.Join(", ", courses.Select(c => c.ToString()).Take(3)) + " ...";
         }
      }
   }
}