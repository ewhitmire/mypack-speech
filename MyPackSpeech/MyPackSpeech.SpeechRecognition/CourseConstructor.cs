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
         String Department = semantics[Slots.Department.ToString()].Value.ToString();
         int Number = (int)semantics[Slots.Number.ToString()].Value;

         IFilter<Course> filter = CourseFilter.DeptAbv(Department).And(CourseFilter.Number(Number, Operator.EQ));
         return CourseCatalog.Instance.GetCourses(filter).First<Course>();
      }

      public static ScheduledCourse ContructScheduledCourse(SemanticValue semantics)
      {
         Course course = ContructCourse(semantics);
         Semester sem = (Semester)Enum.Parse(typeof(Semester), semantics[Slots.Semester.ToString()].Value.ToString(), true);
         int year = int.Parse(semantics[Slots.Year.ToString()].Value.ToString());
         ScheduledCourse sCourse = new ScheduledCourse(course, sem, year);
         return sCourse;
      }



   }
}
