using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech.SpeechRecognition
{
   interface IAction
   {
      Student Student { get; }
      void Inform(SemanticValue sem, Student student);
      bool Perform();
      void Undo();        
   }
}
