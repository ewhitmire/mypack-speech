using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition
{
   public class InfoPaneSetArgs : EventArgs
   {
      public readonly string Text;
      public InfoPaneSetArgs(String text)
      {
         this.Text = text;
      }
   }
}
