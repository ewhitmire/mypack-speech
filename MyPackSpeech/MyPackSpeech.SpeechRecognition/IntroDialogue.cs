using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPackSpeech.DataManager;
using System.Speech.Recognition;

namespace MyPackSpeech.SpeechRecognition
{
   public class IntroDialogue : IDialogueManager
   {
      private static IntroDialogue instance = null;
      public static IntroDialogue Instance
      {
         get
         {
            if (instance == null)
               instance = new IntroDialogue();
            return instance;
         }
      }
      private event EventHandler onComplete;
      public event EventHandler OnComplete { add { onComplete += value; } remove { onComplete -= value; } }

      public void Completed()
      {
         var evt = onComplete;
         if (evt != null)
            evt(this, EventArgs.Empty);
      }

      public int GradYear;
      private Slots expecting;

      public void StartInteraction()
      {
         RecoManager.Instance.SetGrammarMode(GrammarModes.IntroductionGrammar);
         RecoManager.Instance.Say("Welcome to the MyPack Degree Planner. Before we begin, I need to know a little bit about you.");
         RecoManager.Instance.Say("What is your intended major?");
         expecting = Slots.Major;
      }

      public void promptForGradYear()
      {
         RecoManager.Instance.Say("Excellent choice! When are you planning on graduating?");
         expecting = Slots.GradYear;
      }
      public void repromptForGradYear()
      {
         RecoManager.Instance.Say("Ok When are you planning on graduating?");
         expecting = Slots.GradYear;
      }

      public void ProcessResult(RecognitionResult result)
      {
         if (result.Semantics.ContainsKey(expecting.ToString()))
         {
            if (result.Semantics.ContainsKey(Slots.Major.ToString()))
            {
               // ignore major
               promptForGradYear();
            }
            else if (result.Semantics.ContainsKey(Slots.GradYear.ToString()))
            {
               String year = result.Semantics[Slots.GradYear.ToString()].Value.ToString();
               int.TryParse(year, out GradYear);
               confirmGradYear();
            }
            else if (result.Semantics.ContainsKey(Slots.YesNo.ToString()))
            {
               if (result.Semantics[Slots.YesNo.ToString()].Value.ToString().Equals("yes"))
               {
                  gradYearConfirmed();
               }
               else
               {
                  repromptForGradYear();
               }
            }
         }
      }

      private void gradYearConfirmed()
      {
         RecoManager.Instance.Say("Perfect. Give me a moment to set everything up.");
         Completed();
      }

      private void confirmGradYear()
      {
         RecoManager.Instance.Say("Ok, I'll set your graduation date to " + GradYear);
         RecoManager.Instance.Say("Is this correct?");
         expecting = Slots.YesNo;
      }
   }
}
