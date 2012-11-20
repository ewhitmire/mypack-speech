using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition
{
   public class ViewChangeArgs : EventArgs
   {
      public Views view;
      public ViewChangeArgs(Views v)
      {
         view = v;
      }
   }
}
