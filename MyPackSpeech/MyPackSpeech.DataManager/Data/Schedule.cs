using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class Schedule
   {
      public List<ScheduledCourse> Courses { get; private set; }
      public readonly Student Student;

      public Schedule(Student student)
      {
         Student = student;
         Courses = new List<ScheduledCourse>();
      }
   }
}
