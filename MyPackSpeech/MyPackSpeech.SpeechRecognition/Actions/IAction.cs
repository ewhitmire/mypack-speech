using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;

namespace MyPackSpeech.SpeechRecognition
{
   interface IAction
   {
      void Inform(SemanticValue sem);
      bool Perform();
      void Undo();

      
   }
}
