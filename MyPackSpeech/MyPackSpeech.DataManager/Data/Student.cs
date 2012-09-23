using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class Student
   {
      public  List<ScheduledCourse> ScheduledCourses { get; private set;}
      public List<DegreeRequirement> Requirements { get; private set; }

      public Student()
      {
         ScheduledCourses = new List<ScheduledCourse>();
         Requirements = new List<DegreeRequirement>();
      }
      /// <summary>
      /// return the scheduled courses that meet a specific requirement
      /// </summary>
      public IEnumerable<ScheduledCourse> RequiredCourses
      {
         get
         {
            //get courses with a non-null requirement in the requirements list
            return from c in ScheduledCourses
                   where c.Requirement != null && Requirements.Contains(c.Requirement)
                   select c;
         }
      }

      /// <summary>
      /// gets the requirements with an empty fullfillment slot
      /// </summary>
      public IEnumerable<DegreeRequirement> Unfullfilled
      {
         get
         {

            return from r in Requirements
                   where r.FullFillment == null || !ScheduledCourses.Contains(r.FullFillment)
                   select r;
         }
      }
   }
}
