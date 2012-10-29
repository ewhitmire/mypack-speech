using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class AddAction : IAction
   {
      public Student Student { get; private set; }  
      SemanticValue course;
      public void Inform(SemanticValue sem, Student student)
      {
         Student = student;
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
      }
   }
}
