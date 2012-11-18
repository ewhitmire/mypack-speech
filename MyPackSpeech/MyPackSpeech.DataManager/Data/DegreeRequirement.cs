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
   }
}
