using MyPackSpeech.DataManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   public abstract class BaseAction : IAction
   {
      public DataManager.Data.Student Student { get; protected set; }

      public SemanticValueDict Semantics { get; protected set;}

      public bool Inform(SemanticValueDict sem, Student student)
      {
         Student = student;
         if (Semantics == null)
         {
            Semantics = sem;
         }
         else
         {
            foreach (KeyValuePair<String, SemanticValueDict> pair in sem)
            {
               Semantics[pair.Key] = pair.Value;
            }
         }

         return ValidateCurrentData();
      }

      protected virtual bool ValidateCurrentData() { return true; }

      public abstract bool Perform();

      public virtual void GiveConfirmation()
      {
         RecoManager.Instance.Say("OK");
      }

      public static String MakeCourseNameForSpeech(SemanticValueDict semantics)
      {

         String dept = String.Join(" ", semantics.GetSlot(Slots.Department).ToCharArray());
         String number = semantics.GetSlot(Slots.Number);
         return dept + " " + number;
      }

      protected virtual void PromptForMissing(SemanticValueDict semantics, List<Slots> missing)
      {
         if (missing.Count == 4)
         {
            RecoManager.Instance.Say("What would you like to take?");
         }
         else
         {
            if (missing.Contains(Slots.Semester) || missing.Contains(Slots.Year))
            {
               RecoManager.Instance.Say("When would you like to take this course?");
            }
            if (missing.Contains(Slots.Department))
            {
               if (missing.Contains(Slots.Number))
               {
                  RecoManager.Instance.Say("Which course was that?");
               }
               else
               {
                  RecoManager.Instance.Say("What department is this course in?");
               }
            }
         }
      }

      public abstract void Undo();
   }
}
