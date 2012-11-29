using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class SaveLoadAction : BaseAction
   {
      public override bool Perform()
      {
         if (CommandTypes.Save.Equals(Semantics.GetCommand()))
         {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            if (dlg.ShowDialog().GetValueOrDefault(false))
               ActionManager.Instance.CurrStudent.SaveSchedule(dlg.FileName);
         }
         else if (CommandTypes.Load.Equals(Semantics.GetCommand()))
         {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog().GetValueOrDefault(false))
               ActionManager.Instance.CurrStudent.LoadSchedule(dlg.FileName);
         }
         // prevent push to undo stack
         return false;
      }
      public override void Undo()
      {
      }
   }
}
