using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;

namespace MyPackSpeech.SpeechRecognition
{
   public class SemanticValueDict : Dictionary<String, SemanticValueDict>
   {
      public static SemanticValueDict FromSemanticValue(SemanticValue sem)
      {
         if (sem.Value != null)
         {
            return new SemanticValueDict(sem.Value);
         }
         SemanticValueDict dict = new SemanticValueDict();
         foreach (KeyValuePair<String, SemanticValue> pair in sem)
         {
            dict.Add(pair.Key, FromSemanticValue(pair.Value));
         }
         return dict;
      }

      public SemanticValueDict()
      {
      }

      public SemanticValueDict(object o)
      {
         Value = o;
      }

      public String GetSlot(Slots s)
      {
         if (!HasSlot(s))
         {
            return null;
         }
         return this[s.ToString()].Value.ToString();
         
      }
      public bool HasSlot(Slots s)
      {
         return this.Keys.Contains(s.ToString());
      }
      public object Value;
   }
}
