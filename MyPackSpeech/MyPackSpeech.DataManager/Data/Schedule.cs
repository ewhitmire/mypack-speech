using System;
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
   }
}
