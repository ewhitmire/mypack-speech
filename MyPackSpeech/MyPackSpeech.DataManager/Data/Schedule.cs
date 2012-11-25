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

      public Boolean Contains(Course course){
         foreach (ScheduledCourse myCourse in Courses) {
            if (myCourse.Course.Equals(course)) {
               return true;
            }
         }
         return false;
      }

      public List<ScheduledCourse> GetBeforeClass(ScheduledCourse course) {
         List<ScheduledCourse> beforeCourses = new List<ScheduledCourse>();

         foreach(ScheduledCourse c in Courses){
            if(course.isPreviousCourse(c)){
               beforeCourses.Add(c);
            }
         }
         return beforeCourses;
      }

      public List<IFilter<Course>> GetMissingPreReqs(ScheduledCourse course)
      {
         List<IFilter<Course>> missing = new List<IFilter<Course>>();
         List<ScheduledCourse> previousCourses = this.GetBeforeClass(course);

         System.Console.WriteLine("PreviousCourses");
         foreach (ScheduledCourse c in previousCourses)
         {
            System.Console.WriteLine("Previous: " + c.Course.DeptAbv + c.Course.Number);
         }
         //TODO: BrianR get prereqs using course requirements and registered courses
         foreach (var prereq in course.Course.Prerequisites)
         {
            int count = (from c in previousCourses
                         where prereq.Matches(c.Course)
                         select c.Course).Count();
            if (count == 0)
            {
               missing.Add(prereq);
            }
         }

         return missing;
      }
   }
}
