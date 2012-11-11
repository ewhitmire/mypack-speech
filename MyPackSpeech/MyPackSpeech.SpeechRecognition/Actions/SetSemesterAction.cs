using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class SetSemesterAction : BaseAction
   {
      public ScheduledCourse Course { get; private set; }

      override public bool Perform()
      {
         List<Slots> missing = new List<Slots>();//CourseConstructor(semantics);

         if (missing.Count > 0)
         {
            PromptForMissing(semantics, missing);
            return false;
         }
         ActionManager.Instance.SetSemester(semantics);
                
         return false;
      }

      override protected void PromptForMissing(SemanticValueDict semantics, List<Slots> missing)
      {
         
      }

      override public void Undo()
      {
         if (Course != null)
            Student.Schedule.Courses.Remove(Course);
      }
   }
}
