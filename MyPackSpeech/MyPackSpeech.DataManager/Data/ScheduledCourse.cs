﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class ScheduledCourse
   {
      public Course Course { get; private set; }
      public String CourseName
      {
         get
         {
            if (Course != null)
            {
               return Course.Name;
            }
            return "";
         }
      }
      public int Credits
      {
         get
         {
            if (Course != null)
            {
               return Course.Credits;
            }
            return 0;
         }
      }
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

      public Boolean isPreviousCourse(ScheduledCourse otherCourse) {
         //Returns true of otherCourse has been finished before this starts.

         if (this.Year > otherCourse.Year) {
            return true;
         }
         else if (this.Year == otherCourse.Year)
         {
            return (this.Semester == Semester.Fall && otherCourse.Semester == Semester.Spring);
         }
         else 
         {
            return false;
         }
      }

      public override string ToString()
      {
         return string.Format("{0} {1} {2}", Semester, Year, Course);
      }
      public override bool Equals(object obj)
      {
         ScheduledCourse other = obj as ScheduledCourse;
         if ((object)other == null)
         {
            return false;
         }
         return other.Year == Year && other.Semester == Semester && other.Course.Equals(Course);
      }

      public override int GetHashCode()
      {
         return Year ^ Semester.GetHashCode() ^ Course.GetHashCode();
      }
   }
}
