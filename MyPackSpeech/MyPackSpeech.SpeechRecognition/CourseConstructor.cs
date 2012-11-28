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
      public static Course ContructCourse(SemanticValueDict semantics)
      {
         if (semantics.HasSlot(Slots.CourseName))
         {
            String courseName = semantics.GetSlot(Slots.CourseName);
            Course course = CourseCatalog.Instance.Courses.Find(c => c.ToString().Equals(courseName));
            return course;
         }
         else if (semantics.HasSlot(Slots.Department) && semantics.HasSlot(Slots.Number))
         {
            String Department = semantics[Slots.Department.ToString()].Value.ToString();
            int Number = (int)semantics[Slots.Number.ToString()].Value;
            IFilter<Course> filter = CourseFilter.DeptAbv(Department).And(CourseFilter.Number(Number, Operator.EQ));
            return CourseCatalog.Instance.GetCourses(filter).FirstOrDefault();
         }
         else if (ActionManager.Instance.CurrentCourse != null)
         {
            return ActionManager.Instance.CurrentCourse;
         }
         return null;
      }

      /// <summary>
      /// Contructs the scheduled course, will get the year and semester from the ActionManager
      /// if they are not part of the semantic value
      /// </summary>
      /// <param name="semantics">The semantics.</param>
      /// <returns></returns>
      public static ScheduledCourse ContructScheduledCourse(SemanticValueDict semantics)
      {
         Course course = ContructCourse(semantics);
         if (course == null)
         {
            return null;
         }
         Semester? sem = GetSemester(semantics, ActionManager.Instance.CurrentSemester);
         int? year = GetYear(semantics, ActionManager.Instance.CurrentYear);

         if (year == null)
            throw new InvalidOperationException("Should not be call without valid year information");
         if (sem == null)
            throw new InvalidOperationException("Should not be called without semester information");

         ScheduledCourse sCourse = new ScheduledCourse(course, sem.Value, year.Value);
         return sCourse;
      }

      public static Semester? GetSemester(SemanticValueDict semantics, Semester? sem = null)
      {         
         if (semantics.ContainsKey(Slots.Semester.ToString()))
         {
            sem = (Semester)Enum.Parse(typeof(Semester), semantics[Slots.Semester.ToString()].Value.ToString(), true);
         }

         return sem;
      }

      public static int? GetYear(SemanticValueDict semantics, int? year = null)
      {
         if (semantics.ContainsKey(Slots.Year.ToString()))
            year = int.Parse(semantics[Slots.Year.ToString()].Value.ToString());

         return year;
      }

      public static List<Slots> SemanticsContainsCourseData(SemanticValueDict course)
      {
         List<Slots> missing = new List<Slots>();
         if (!course.HasSlot(Slots.CourseName))
         {
            if (!course.ContainsKey(Slots.Department.ToString()))
            {
               missing.Add(Slots.Department);
            }
            if (!course.ContainsKey(Slots.Number.ToString()))
            {
               missing.Add(Slots.Number);
            }
         }
         return missing;
      }

      public static List<Slots> ContainsCourseData(SemanticValueDict course)
      {
         if (ActionManager.Instance.CurrentCourse == null)
         {
            return SemanticsContainsCourseData(course);
         }
         return new List<Slots>();
      }

      public static List<Slots> ContainsScheduledCourseData(SemanticValueDict course, bool ignoreSemester = false)
      {
         List<Slots> missing = ContainsCourseData(course);
         if (!ignoreSemester)
         {
            if (!course.ContainsKey(Slots.Semester.ToString()) && !ActionManager.Instance.CurrentSemester.HasValue)
            {
               missing.Add(Slots.Semester);
            }
            if (!course.ContainsKey(Slots.Year.ToString()) && !ActionManager.Instance.CurrentYear.HasValue)
            {
               missing.Add(Slots.Year);
            }
         }

         return missing;
      }

      internal static bool IsCourseDataValid(SemanticValueDict semantics)
      {
         if (semantics.HasSlot(Slots.CourseName))
         {
            return true;
         }
         else
         {
            String dept = semantics.GetSlot(Slots.Department);
            String num = semantics.GetSlot(Slots.Number);
            IEnumerable<Course> courses = CourseCatalog.Instance.Courses.Where(c => c.Dept.Abv.Equals(dept) && c.Number.Equals(int.Parse(num)));
            return courses.Count() > 0;
         }
      }

      public static bool IsSemesterValid(Semester sem, int year)
      {
         if (year >= ActionManager.Instance.GradYear)
         {
            if (year == ActionManager.Instance.GradYear && sem == Semester.Spring)
            {
               return true;
            }
            else
            {
            return false;
            }
         }
         else if (year <= ActionManager.Instance.GradYear - 4)
         {
            if (year == ActionManager.Instance.GradYear-4 && sem == Semester.Fall)
            {
               return true;
            }
            else
            {
               return false;
            }
         }
         return true;
      }
   }
}
