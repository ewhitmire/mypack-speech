using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class ScheduledCourse
   {
      public Course Course { get; private set; }
      public Semester Semester { get; private set; }
      public int Year { get; private set; }
      public DegreeRequirement Requirement {get;set;}

      public ScheduledCourse(Course course, Semester semester, int year, DegreeRequirement req = null)
      {
         Semester = semester;
         Course = course;
         Year = year;
         Requirement = req;
      }

      public override string ToString()
      {
         return string.Format("{0} {1} {2}", Semester, Year, Course);
      }
   }
}
