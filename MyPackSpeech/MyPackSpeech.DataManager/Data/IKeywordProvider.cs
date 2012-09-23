using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public interface IKeywordProvider
   {
      List<string> KeyWords { get; }
      bool IsMatch(string word);
      double MatchLiklihood(string word);
   }
}
