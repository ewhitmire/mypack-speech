using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
         //TODO: BrianR get prereqs using course requirements and registered courses
         return new List<Course>();
      }
   }
}
