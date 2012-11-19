using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPackSpeech.DataManager.Data.Filter;

namespace MyPackSpeech.DataManager.Data
{
   public class DegreeRequirement
   {
      private ScheduledCourse fulfillment;
      public IFilter<Course> CourseRequirement { get; set; }
      public DegreeRequirementCategory Category { get; set; }
      public String Name;

      public DegreeRequirement()
      {
      }

      public ScheduledCourse Fulfillment
      {
         get { return fulfillment; }
         set
         {
            if (fulfillment != value)
            {
               //clear up old references
               if (fulfillment != null && fulfillment.Requirement == this)
                  fulfillment.Requirement = null;

               fulfillment = value;

               //complete link
               if (fulfillment != null)
                  fulfillment.Requirement = this;
            }
         }
      }

      public override string ToString()
      {
         if (Fulfillment != null)
            return string.Format("{0} => {1}", CourseRequirement, Fulfillment);

         return CourseRequirement.ToString();
      }

      public string ToSpeechString()
      {
         if (Name != null)
         {
            return Name;
         }
         IEnumerable<Course> courses = CourseCatalog.Instance.GetCourses(CourseRequirement);
         if (courses.Count() == 1)
         {
            return courses.First().Name;
         }
         return Category.Name;
      }

      public string ToPrintedString() {

         IEnumerable<Course> courses = CourseCatalog.Instance.GetCourses(CourseRequirement);
         
         if (courses.Count() == 1)
         {
            Course match = courses.First();
            return match.DeptAbv + match.Number + " - " + match.Name;
         }

         List<Course> orRequirements = courses.ToList();
         string str = "        " + orRequirements[0].DeptAbv + orRequirements[0].Number + " - " + orRequirements[0].Name;
         for (int i = 1; i < orRequirements.Count; i++)
         {
            Course match = orRequirements[i];
            str += "\n  OR " + match.DeptAbv + match.Number + " - " + match.Name;
         }
         return str;
      
      }
   }
}
