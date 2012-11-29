using MyPackSpeech.DataManager;
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
         Choices majors = new Choices();
         majors.Add(new SemanticResultValue("Computer Science", "CSC"));

         SemanticResultKey major = new SemanticResultKey(Slots.Major.ToString(), majors);

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
         options.Add(major);
         options.Add(year);
         options.Add(yesNo);

         GrammarBuilder builder = new GrammarBuilder();
         builder.Append(options);
         grammar = new Grammar(builder);
      }
   }
}
