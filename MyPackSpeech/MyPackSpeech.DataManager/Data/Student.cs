using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class Student
   {
      public event EventHandler ScheduleChanged;
      public event EventHandler BookmarksChanged;
      public Schedule Schedule { get; private set; }
      public DegreeProgram Degree { get; private set; }
      public List<Course> bookmarks = new List<Course>();

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
         foreach (DegreeRequirement req in Degree.Requirements.Where(c => c.Fulfillment == null))
         {
            if (req.CourseRequirement.Matches(course.Course))
            {
               req.Fulfillment = course;
            }
         }
         OnScheduleChanged();
      }

      public void AddBookmark(Course course)
      {
         this.bookmarks.Add(course);
         OnBookmarksChanged();
      }

      public void RemoveBookmark(Course course)
      {
         this.bookmarks.Remove(course);
         OnBookmarksChanged();
      }

      public ScheduledCourse MoveCourse(ScheduledCourse course)
      {
         //Remove existing course then add the new course
         //Return exiting course for undo
         ScheduledCourse existing = FindCourse(course.Course);

         if (existing != null)
         {
            RemoveCourse(existing.Course);
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
         return Schedule.Courses.ToList().Find(c => c.Course.Equals(course));
      }

      private void OnScheduleChanged()
      {
         EventHandler evt = ScheduleChanged;
         if (evt != null)
            evt(this, EventArgs.Empty);
      }

      private void OnBookmarksChanged()
      {
         EventHandler evt = BookmarksChanged;
         if (evt != null)
            evt(this, EventArgs.Empty);
      
      }
	  public void RemoveCourse(Course Course)
	  {

        ScheduledCourse myCourse = Schedule.Courses.Where(c => c.Course.Equals(Course)).FirstOrDefault();
        if (myCourse != null)
        {
           Schedule.Courses.Remove(myCourse);
           DegreeRequirement req = Degree.Requirements.Where(c => Course.Equals(c.Fulfillment)).FirstOrDefault();
           if (req != null)
           {
              req.Fulfillment = null;
           }
           OnScheduleChanged();
        }
        else
        {
           if(bookmarks.Contains(Course)){
            RemoveBookmark(Course);
           } 
        }
        

	  }

   }
}
