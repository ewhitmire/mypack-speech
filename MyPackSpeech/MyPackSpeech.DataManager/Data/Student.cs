using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class Student
   {
      public event EventHandler ScheduleChanged;
      public Schedule Schedule { get; private set; }
      public DegreeProgram Degree { get; private set; }

      public Student(DegreeProgram degree)
      {
         Schedule = new Schedule(this);
         Degree = degree;
      }
      /// <summary>
      /// return the scheduled courses that meet a specific requirement
      /// </summary>
      public IEnumerable<ScheduledCourse> RequiredCourses
      {
         get
         {
            //get courses with a non-null requirement in the requirements list
            return from c in Schedule.Courses
                   where c.Requirement != null && Degree.Requirements.Contains(c.Requirement)
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

            return from r in Degree.Requirements
                   where r.Fulfillment == null || !Schedule.Courses.Contains(r.Fulfillment)
                   select r;
         }
      }

      public void AddCourse(ScheduledCourse course)
      {
         Schedule.Courses.Add(course);
         OnScheduleChanged();
      }

      public ScheduledCourse MoveCourse(ScheduledCourse course)
      {
         //Remove existing course then add the new course
         //Return exiting course for undo
         ScheduledCourse existing = FindCourse(course.Course);

         if (existing != null)
         {
            RemoveCourse(existing);
         }

         AddCourse(course);
         
         OnScheduleChanged();
         return existing;
      }

      public bool IsTaking(Course course)
      {
         return FindCourse(course) != null;
      }

      public ScheduledCourse FindCourse(Course course)
      {
         return Schedule.Courses.Find(c => c.Course.Equals(course));
      }

      private void OnScheduleChanged()
      {
         EventHandler evt = ScheduleChanged;
         if (evt != null)
            evt(this, EventArgs.Empty);
      }

	  public void RemoveCourse(ScheduledCourse Course)
	  {
		  Schedule.Courses.Remove(Course);
		  OnScheduleChanged();
	  }
   }
}
