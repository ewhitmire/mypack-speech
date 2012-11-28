using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;

namespace MyPackSpeech.DataManager
{
   public interface IDialogueManager
   {
      void ProcessResult(RecognitionResult result);
   }
}
