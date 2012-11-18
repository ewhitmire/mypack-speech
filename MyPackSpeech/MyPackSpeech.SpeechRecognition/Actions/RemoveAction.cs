using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class RemoveAction : BaseAction
   {
		public ScheduledCourse Course { get; private set; }

      override public bool Perform()
      {
         List<Slots> missing = CourseConstructor.ContainsScheduledCourseData(Semantics, true);


			if (missing.Count > 0)
			{
				PromptForMissing(Semantics, missing);
				return false;
			}

         Course sCourse= CourseConstructor.ContructCourse(Semantics);


         if (sCourse != null)
         {
            if (!Student.Schedule.Contains(sCourse) && !Student.bookmarks.Contains(sCourse))
            {
               RecoManager.Instance.Say("You must add that course before attempting to remove it.");
            }
            else
            {
               Student.RemoveCourse(sCourse);
               return true;
            }
         }

         return false;
      }

      override protected void PromptForMissing(SemanticValueDict semantics, List<Slots> missing)
      {
         if (missing.Contains(Slots.Department) || missing.Contains(Slots.Number))
         {
            RecoManager.Instance.Say("What course are you trying to remove?");
         }
      }


      override public void Undo()
		{
			if (Course != null)
			{
				Student.AddCourse(Course);
			}
		}
	}
}
