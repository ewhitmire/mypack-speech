using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class AddAction : BaseAction
   {
      public ScheduledCourse Course { get; private set; }

      override public bool Perform()
      {
         List<Slots> missing = CourseConstructor.ContainsScheduledCourseData(semantics);

         if (missing.Count > 0)
         {
            PromptForMissing(semantics, missing);
            return false;
         }

         Course = CourseConstructor.ContructScheduledCourse(semantics);
         if (Course == null)
         {
            correctCourse();
            return false;
         }
         else
         {
            switch (Course.Semester) { 
               case Semester.Fall:
                  if (!Course.Course.fall) {
                     ActionManager.Instance.notOffered(semantics, Course.Semester);
                     return false;
                  }
                  break;
               case Semester.Spring:
                  if (!Course.Course.spring) {
                     ActionManager.Instance.notOffered(semantics, Course.Semester);
                     return false;
                  }
                  break;
               default:
                  return false;

            }

            Student.AddCourse(Course);
            return true;
         }
      }
      protected override bool ValidateCurrentData()
      {
         bool allGood = base.ValidateCurrentData();
         if (CourseConstructor.ContainsCourseData(semantics).Count == 0)
         {
            if (!CourseConstructor.IsCourseDataValid(semantics))
            {
               correctCourse();
               allGood = false;
            }
         }
         return allGood;
      }
      private void correctCourse()
      {
         String course = MakeCourseNameForSpeech(semantics);
         RecoManager.Instance.Say(course + " is not a valid course");
         semantics.Remove(Slots.Department.ToString());
         semantics.Remove(Slots.Number.ToString());

      }
      override public void GiveConfirmation()
      {
         String course = MakeCourseNameForSpeech(semantics);
         RecoManager.Instance.Say("Ok, I added "+course);
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
               RecoManager.Instance.Say("When would you like to take this course?");
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
            Student.Schedule.Courses.Remove(Course);
      }
   }
}
