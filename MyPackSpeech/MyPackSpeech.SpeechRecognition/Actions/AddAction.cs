using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class AddAction : IAction
   {
      SemanticValue course;
      public void Inform(SemanticValue sem)
      {
         if (sem.ContainsKey(Slots.Course1.ToString()))
         {
            course = sem[Slots.Course1.ToString()];
         }
      }
      public bool Perform()
      {
         List<Slots> missing = ActionManager.ValidateCourse(course);

         if (missing.Count > 0)
         {
            ActionManager.Instance.PromptForMissing(course, missing);
            return false;
         }
         return true;
      }

      public void Undo()
      {
<<<<<<< HEAD
         throw new NotImplementedException();
=======
>>>>>>> aff0d614a10c252dff43f38588170c6663bd5e16
      }
   }
}
