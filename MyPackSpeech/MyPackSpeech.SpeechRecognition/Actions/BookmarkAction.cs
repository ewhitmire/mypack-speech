using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;
using MyPackSpeech.DataManager.Data.Filter;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class BookmarkAction : BaseAction
   {
      public Course Course { get; private set; }

      override public bool Perform()
      {
         Course = CourseConstructor.ContructCourse(Semantics);

         if (Course != null)
         {

            //RecoManager.Instance.Say("Ok, I bookmarked " + Course);
            Student.AddBookmark(Course);
            return true;
         }
         
         return false;
         //}
      }
      protected override bool ValidateCurrentData()
      {
         bool allGood = base.ValidateCurrentData();
         if (CourseConstructor.ContainsCourseData(Semantics).Count == 0)
         {
            if (DialogManager.Instance.CurrentCourse == null && !CourseConstructor.IsCourseDataValid(Semantics))
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
         String course = MakeCourseNameForSpeech(Course);
         RecoManager.Instance.Say("Ok, I bookmarked " +  course);
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
       //  if (Course != null)
       //     Student.Schedule.Courses.Remove(Course);
      }
   }
}