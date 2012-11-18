using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech;
using System.Speech.Synthesis;

namespace MyPackSpeech.SpeechRecognition
{
   public static class SpeechUtils
   {

      public static string MakeSpeechList(IEnumerable<String> reqNames)
      {
         // everything but final ", and ..."
         String text = String.Join(", ", reqNames.Take(reqNames.Count()-1));
         text = text + ", and "+reqNames.Last();
         return text;
      }
   }
}
