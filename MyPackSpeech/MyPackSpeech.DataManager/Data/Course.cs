using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MyPackSpeech.DataManager.Search;

namespace MyPackSpeech.DataManager.Data
{
   public class Course : IKeywordProvider, MyPackSpeech.DataManager.Data.ICourse
   {
      public Department Dept { get; private set; }
      public int Number { get; private set; }
      public string Name { get; private set; }
      [Browsable(false)]
      public string Description { get; private set; }
      [Browsable( false)]
      public List<CourseFilter> Prerequisites { get; private set; }

      [DisplayName("Prereqs")]
      public string PrereqDesc
      {
         get
         {
            return string.Join(", ", Prerequisites.Select(p => p.ToString()));
         }
      }
      public Course(Department dept, string name, int number, string desc, params CourseFilter[] prereqs)
      {
         checkArguments(dept, number);

         Dept = dept;
         Number = number;
         Name = name;
         Description = desc ?? string.Empty;
         Prerequisites = new List<CourseFilter>(prereqs);

         setKeyWords();
      }
      
      public override string ToString()
      {
         return string.Format("{0}{1}", Dept, Number);
      }

      private static void checkArguments(Department dept, int number)
      {
         if (dept == null)
            throw new ArgumentException("Dept can't be null");
         if (number < 100)
            throw new ArgumentException("number has to be >=100");
      }

      private void setKeyWords()
      {
         KeyWords = KeywordGenerator.GetKeywords(Description);
      }

      #region IKeywordProvider
      [Browsable(false)]
      public List<string> KeyWords { get; private set; }

      public bool IsMatch(string word)
      {
         return KeyWords.Find(s => s.Equals(word, StringComparison.CurrentCultureIgnoreCase)).Count() > 0;
      }

      public double MatchLiklihood(string word)
      {
         return IsMatch(word)
            ? 1
            : 0;
      }
      #endregion IKeywordProvider
   }
}