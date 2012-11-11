using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class UnknownAction : BaseAction
   {
      public override bool Perform()
      {
         if (CourseConstructor.ContainsCourseData(Semantics).Count == 0)
         {
            String courseName = BaseAction.MakeCourseNameForSpeech(Semantics);
            RecoManager.Instance.Say("What would you like to do with " + courseName);
         }
         else
         {
            RecoManager.Instance.Say("I'm not sure what you are trying to do.");
         }
         return false;
      }

      public override void Undo()
      {
         throw new NotImplementedException();
      }
   }
}
