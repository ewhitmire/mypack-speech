using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;
using MyPackSpeech.DataManager.Data.Filter;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class AddAction : BaseAction
   {
      public ScheduledCourse Course { get; private set; }

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
            correctCourse();
            return false;
         }
         else
         {
            List<IFilter<Course>> missingClasses = Student.Schedule.GetMissingPreReqs(Course);

            if (missingClasses.Count > 0)
            {
               ActionManager.Instance.InformPreReqs(Course.Course, missingClasses);
               return false;
            }
            else
            {
               if ((Course.Year < 2012 || Course.Year > 2016) ||
                  (Course.Year == 2012 && Course.Semester == Semester.Spring) ||
                  (Course.Year == 2016 && Course.Semester == Semester.Fall)) {
                  RecoManager.Instance.Say("That semseter is outside of the current range.");
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

               Student.AddCourse(Course);
               Student.RemoveBookmark(Course.Course);
               return true;
            }
         }
      }
      protected override bool ValidateCurrentData()
      {
         bool allGood = base.ValidateCurrentData();
         if (CourseConstructor.ContainsCourseData(Semantics).Count == 0)
         {
            if (ActionManager.Instance.CurrentCourse == null && !CourseConstructor.IsCourseDataValid(Semantics))
            {
               correctCourse();
               allGood = false;
            }
         }
         return allGood;
      }

      private void correctCourse()
      {
         String course = MakeCourseNameForSpeech(Semantics);
         RecoManager.Instance.Say(course + " is not a valid course");
         Semantics.Remove(Slots.Department.ToString());
         Semantics.Remove(Slots.Number.ToString());
      }

      override public void GiveConfirmation()
      {
         String course = MakeCourseNameForSpeech(Course.Course);
         RecoManager.Instance.Say("Ok, I added " + course);
      }

      override protected void PromptForMissing(SemanticValueDict semantics, List<Slots> missing)
      {
         if (missing.Count == 4)
         {
            RecoManager.Instance.Say("What would you like to take?");
         }
         else
         {
            if (missing.Contains(Slots.Semester) || missing.Contains(Slots.Year))
            {
               if (!missing.Contains(Slots.Semester))
               {
                  RecoManager.Instance.Say("In what year would you like to take this course?");
               }
               else if (!missing.Contains(Slots.Year))
               {
                  RecoManager.Instance.Say("In which semester would you like to take this course?");
               }
               else
               {
                  RecoManager.Instance.Say("When would you like to take this course?");
               }
            }
            if (missing.Contains(Slots.Department))
            {
               if (missing.Contains(Slots.Number))
               {
                  RecoManager.Instance.Say("Which course was that?");
               }
               else
               {
                  RecoManager.Instance.Say("What department is this course in?");
               }
            }
         }
      }

      override public void Undo()
      {
         if (Course != null)
            Student.RemoveCourse(Course.Course);
      }
   }
}