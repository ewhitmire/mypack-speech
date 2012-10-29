using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class SwapAction : IAction
   {
      SemanticValue course1;
      SemanticValue course2;

      public void Inform(SemanticValue sem)
      {
         if (sem.ContainsKey(Slots.Course1.ToString()))
         {
            course1 = sem[Slots.Course1.ToString()];
         }
         if (sem.ContainsKey(Slots.Course2.ToString()))
         {
            course2 = sem[Slots.Course2.ToString()];
         }
      }

      public bool Perform()
      {
         List<Slots> missing1 = ActionManager.ValidateExistingCourse(course1);
         List<Slots> missing2 = ActionManager.ValidateExistingCourse(course1);

         if (missing1.Count > 0)
         {
            ActionManager.Instance.PromptForMissing(course1, missing1);
            return false;
         }
         if (missing2.Count > 0)
         {
            ActionManager.Instance.PromptForMissing(course2, missing1);
            return false;
         }
         return true;
      }

      public void Undo()
      {
         //throw new NotImplementedException();
      }
   }
}
