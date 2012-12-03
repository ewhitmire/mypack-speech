using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   public class HelpAction : BaseAction
   {
      public override bool Perform()
      {
         DialogManager.Instance.ShowHelp();
         return false;
      }

      public override void Undo()
      {
         
      }
   }
}
