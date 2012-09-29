using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Search
{
   public static class KeywordGenerator
   {
      private static readonly char[] SplitChars = new char[] { ' ', '\n', '\t', '\r' };
      
      public static List<string> GetKeywords(string sentence)
      {
         char[] arr = sentence.ToCharArray();

         arr = Array.FindAll<char>(arr, c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)));
         sentence = new string(arr);

         List<string> keyWords = (from s in sentence.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim().ToLower())
                                  where !string.IsNullOrEmpty(s)
                                  select s).ToList();
         
         return keyWords;
      }
   }
}
