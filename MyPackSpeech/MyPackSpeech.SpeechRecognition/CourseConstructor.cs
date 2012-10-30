using MyPackSpeech.DataManager.Data;
using MyPackSpeech.DataManager.Data.Filter;
using MyPackSpeech.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;

namespace MyPackSpeech.SpeechRecognition
{
   public static class CourseConstructor
   {
      public static Course ContructCourse(SemanticValue semantics)
      {
         String Department = semantics[Slots.Department.ToString()].ToString();
         int Number = (int)semantics[Slots.Number.ToString()].Value;

         IFilter<Course> filter = CourseFilter.DeptAbv(Department).And(CourseFilter.Number(Number, Operator.EQ));
         CourseCatalog.Instance.Filter = filter;
         return CourseCatalog.Instance.FilteredCourses[0];
      }

      public static ScheduledCourse ContructScheduledCourse(SemanticValue semantics)
      {
         Course course = ContructCourse(semantics);
         Semester sem = (Semester) Enum.Parse(typeof(Semester),  semantics[Slots.Semester.ToString()].ToString());
         int year = (int)semantics[Slots.Year.ToString()].Value;
         ScheduledCourse sCourse = new ScheduledCourse(course, sem, year);
         return sCourse;
      }



   }
}
