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
         if (Semantics.HasSlot(Slots.KeyWords))
         {
            String keyword = Semantics.GetSlot(Slots.KeyWords);
            IEnumerable<Course> courses = CourseCatalog.Instance.Courses.AsParallel().Where(c => c.KeyWords.Contains(keyword)).OrderBy(c=>c.ToString());

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
         return false;
      }

      public override void Undo()
      {
         throw new NotImplementedException();
      }
   }
}
