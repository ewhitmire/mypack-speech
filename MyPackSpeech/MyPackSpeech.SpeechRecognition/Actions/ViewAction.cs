using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class ViewAction : BaseAction
   {
      public override bool Perform()
      {
         if (Semantics.HasSlot(Slots.ViewName))
         {
            Views view = (Views)Enum.Parse(typeof(Views), Semantics.GetSlot(Slots.ViewName));
            ActionManager.Instance.SwitchView(view);
         }
         return false;
      }

      public override void Undo()
      {
         
      }
   }
}
