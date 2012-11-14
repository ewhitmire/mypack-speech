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

      public List<IFilter<Course>> GetMissingPreReqs(ScheduledCourse course)
      {
		  List<IFilter<Course>> missing = new List<IFilter<Course>>();
         //TODO: BrianR get prereqs using course requirements and registered courses
		  foreach (var prereq in course.Course.Prerequisites)
		  {
			  var matches = from c in Courses
							where prereq.Matches(c.Course)
							select c.Course;
			  if (matches.Count() == 0)
			  {
				  missing.Add(prereq);
			  }
		  }

         return missing;
      }

      private void addPreReqToMissing(IFilter<Course> prereq)
      {
      }
   }
}
