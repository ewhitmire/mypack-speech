using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MyPackSpeech.DataManager.Data.Filter;

namespace MyPackSpeech.DataManager.Data
{
   public class Schedule
   {      
      public ObservableCollection<ScheduledCourse> Courses { get; private set; }
      public readonly Student Student;

      public Schedule(Student student)
      {
         Student = student;
         Courses = new ObservableCollection<ScheduledCourse>();
      }

      public List<Course> GetMissingPreReqs(ScheduledCourse course)
      {
         List<Course> missing = new List<Course>();
         //TODO: BrianR get prereqs using course requirements and registered courses
         
         foreach (var prereq in course.Course.Prerequisites)
         {
            int count = Courses.Where(c => prereq.Matches(c.Course)).Count();

            if (count > 0)
            {
               addPreReqToMissing(prereq);
            }
         }

         return missing;
      }

      private void addPreReqToMissing(IFilter<Course> prereq)
      {
      }
   }
}
