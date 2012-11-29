using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;
using MyPackSpeech.DataManager.Data.Filter;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class MoveAction : BaseAction
   {
      public ScheduledCourse Course { get; private set; }
      public ScheduledCourse OldCourse { get; private set; }
      
      override public bool Perform()
      {
         List<Slots> missing = CourseConstructor.ContainsScheduledCourseData(Semantics);

         if (missing.Count > 0)
         {
            PromptForMissing(Semantics, missing);
            return false;
         }

         Course = CourseConstructor.ContructScheduledCourse(Semantics);

         if (Course == null)
         {
            RecoManager.Instance.Say("What course are you trying to move?");
            return false;
         }

         if (!Student.Schedule.Contains(Course.Course)) {
            RecoManager.Instance.Say("You must add that course to your schedule before moving it.");
            return false;
         }

         if (!CourseConstructor.IsSemesterValid(Course.Semester, Course.Year))
         {
            RecoManager.Instance.Say("That semester is outside of the current range.");
            return false;
         }

         List<IFilter<Course>> missingClasses = Student.Schedule.GetMissingPreReqs(Course);

         if (missingClasses.Count > 0)
         {
            ActionManager.Instance.InformPreReqs(Course, missingClasses);
            return false;
         }

         switch (Course.Semester)
         {
            case Semester.Fall:
               if (!Course.Course.fall)
               {
                  ActionManager.Instance.notOffered(Semantics, Course.Semester);
                  return false;
               }
               break;
            case Semester.Spring:
               if (!Course.Course.spring)
               {
                  ActionManager.Instance.notOffered(Semantics, Course.Semester);
                  return false;
               }
               break;
            default:
               return false;

         }

         OldCourse = Student.MoveCourse(Course);
         return true;
      }

      override public void Undo()
      {
         if (Course != null)
            Student.Schedule.Courses.Remove(Course);
         if (OldCourse != null)
            Student.AddCourse(OldCourse);
      }

      override protected void PromptForMissing(SemanticValueDict semantics, List<Slots> missing)
      {
         if (missing.Contains(Slots.Department) || missing.Contains(Slots.Number))
         {
            RecoManager.Instance.Say("What course are you trying to move?");
         }
         else if (missing.Contains(Slots.Semester) || missing.Contains(Slots.Year))
         {
            RecoManager.Instance.Say("When would you like to take this course?");
         }
      }    
   }
}
