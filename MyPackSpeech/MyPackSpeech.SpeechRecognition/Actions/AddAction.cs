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
      private SemanticValue semantics = null;
      public void Inform(SemanticValue sem, Student student)
      {
         Student = student;
         semantics = sem;
      }
      public bool Perform()
      {
         List<Slots> missing = ActionManager.ValidateCourse(semantics);

         if (missing.Count > 0)
         {
            ActionManager.Instance.PromptForMissing(semantics, missing);
            return false;
         }

         ScheduledCourse sCourse = CourseConstructor.ContructScheduledCourse(semantics);
         Student.AddCourse(sCourse);
         return true;
      }

     

      public void Undo()
      {
      }
   }
}
