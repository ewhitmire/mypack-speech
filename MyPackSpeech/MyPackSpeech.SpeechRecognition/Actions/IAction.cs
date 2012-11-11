using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech.SpeechRecognition
{
   public interface IAction
   {
      Student Student { get; }
      bool Inform(SemanticValueDict sem, Student student);
      bool Perform();
      void Undo();
      void GiveConfirmation();
   }
}