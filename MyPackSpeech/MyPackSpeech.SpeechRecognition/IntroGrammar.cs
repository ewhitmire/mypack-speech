using MyPackSpeech.DataManager;
using MyPackSpeech.DataManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;

namespace MyPackSpeech.SpeechRecognition
{
   public class IntroGrammar
   {
      public Grammar grammar;
      public IntroGrammar()
      {
         List<Department> depts = CourseCatalog.Instance.Departments;
         Choices majors = new Choices();
         foreach (Department major in depts)
         {
            majors.Add(new SemanticResultValue(major.Name, major.Abv));
         }

         SemanticResultKey majorKey = new SemanticResultKey(Slots.Major.ToString(), majors);

         Choices years = new Choices();
         for (int i = 2001; i < 2020; i++)
         {
            years.Add(new SemanticResultValue(i.ToString(), i));
         }
         SemanticResultKey year = new SemanticResultKey(Slots.GradYear.ToString(), years);


         Choices yesOrNo = new Choices();
         yesOrNo.Add(new SemanticResultValue("yes", "yes"));
         yesOrNo.Add(new SemanticResultValue("yeah", "yes"));
         yesOrNo.Add(new SemanticResultValue("yep", "yes"));
         yesOrNo.Add(new SemanticResultValue("no", "no"));
         yesOrNo.Add(new SemanticResultValue("nope", "no"));
         SemanticResultKey yesNo = new SemanticResultKey(Slots.YesNo.ToString(), yesOrNo);

         Choices options = new Choices();
         options.Add(majorKey);
         options.Add(year);
         options.Add(yesNo);

         GrammarBuilder builder = new GrammarBuilder();
         builder.Append(options);
         grammar = new Grammar(builder);
      }
   }
}
