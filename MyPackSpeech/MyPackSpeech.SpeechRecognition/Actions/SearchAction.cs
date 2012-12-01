using MyPackSpeech.DataManager;
using MyPackSpeech.DataManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   public class SearchAction : BaseAction
   {
      public override bool Perform()
      {
         List<String> keywords = gatherWords(Semantics);

         if (keywords.Count > 0)
         {
            IEnumerable<Course> courses = CourseCatalog.Instance.Courses.AsParallel().Where(c => keywords.All(w => c.KeyWords.Contains(w))).OrderBy(c => c.ToString());
            string keyword = String.Join(" and ", keywords.ToArray());
            String paneText = "";
            foreach (Course c in courses)
            {
               paneText += c.DeptAbv + c.Number + " - " + c.Name + "\n";
            }

            ActionManager.Instance.SetInfoPane(paneText);

            if (courses.Count() > 5)
            {
               RecoManager.Instance.Say("I found " + courses.Count() + " " + keyword + " related courses. Check the info pane for a list of possible classes.");
            }
            else if (courses.Count() == 0)
            {
               RecoManager.Instance.Say("I can't find any courses like that");
            }
            else if (courses.Count() != 1)
            {
               IEnumerable<String> classNames = courses.Select<Course, String>(c => c.Name);
               RecoManager.Instance.Say("I found " + courses.Count() + " " + keyword + " related courses, including " + SpeechUtils.MakeSpeechList(classNames));
            }
            else
            {
               IEnumerable<String> classNames = courses.Select<Course, String>(c => c.Name);
               RecoManager.Instance.Say("I only found one " + keyword + " related course, " + SpeechUtils.MakeSpeechList(classNames));
            }
         }

         Semantics.Clear();

         return false;
      }

      private List<string> gatherWords(SemanticValueDict semantics)
      {
         List<string> keywords = new List<string>();

         getWord(semantics, Slots.KeyWords, keywords);
         getWord(semantics, Slots.KeyWords2, keywords);
         getWord(semantics, Slots.KeyWords3, keywords);
         getWord(semantics, Slots.KeyWords4, keywords);
         
         return keywords;
      }

      private static void getWord(SemanticValueDict Semantics, Slots slot, List<string> keywords)
      {
         if (Semantics.HasSlot(slot))
         {
            keywords.Add(Semantics.GetSlot(slot));
         }
      }

      public override void Undo()
      {
      }
   }
}
