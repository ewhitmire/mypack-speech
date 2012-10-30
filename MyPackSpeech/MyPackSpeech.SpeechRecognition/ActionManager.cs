using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Diagnostics;
using MyPackSpeech.SpeechRecognition;
using MyPackSpeech.SpeechRecognition.Actions;
using MyPackSpeech.DataManager.Data;
using MyPackSpeech.DataManager;

namespace MyPackSpeech
{
   public class ActionDetectedEventArgs : System.EventArgs
   {
      public readonly CommandTypes CommandType;
      public readonly Student Student;

      public ActionDetectedEventArgs(CommandTypes t, Student student)
      {
         this.CommandType = t;
         this.Student = student;
      }

   }

   public class ActionManager
   {
      private static ActionManager instance = null;
      public static ActionManager Instance
      {
         get
         {
            if (instance == null)
               instance = new ActionManager();
            return instance;
         }
      }

      // event declaration 
      public event EventHandler<ActionDetectedEventArgs> ActionDetected;

      Stack<IAction> actionHistory = new Stack<IAction>();

      public void ProcessResult(RecognitionResult result)
      {
         if (result.Semantics.ContainsKey("command"))
         {
            ProcessCommand(result);
         }
         else
         {
            ProcessSupplemental(result);
         }
      }

      private void ProcessSupplemental(RecognitionResult result)
      {
         throw new NotImplementedException();
      }

      private void ProcessCommand(RecognitionResult result)
      {
         CommandTypes cmd = (CommandTypes)(result.Semantics["command"].Value);
         bool callEvent = false;

         if (cmd == CommandTypes.Undo)
         {
            if (actionHistory.Count > 0)
            {
               IAction action = actionHistory.Pop();
               action.Undo();
               callEvent = true;
            }
         }
         else
         {
            Type ActionType = cmd.ActionClass();
            if (!ActionType.Equals(typeof(IAction)))
            {

               IAction action = (IAction)Activator.CreateInstance(ActionType);
               action.Inform(result.Semantics, CurrStudent);
               callEvent = action.Perform();
               actionHistory.Push(action);
            }
         }

         if (callEvent && ActionDetected != null)
         {
            ActionDetectedEventArgs args = new ActionDetectedEventArgs(cmd, CurrStudent);
            ActionDetected(this, args);
            RecoManager.Instance.reader.SpeakAsync("Ok");
         }
      }

     
      public void PromptForMissing(SemanticValue context, List<Slots> missing)
      {
         RecoManager.Instance.reader.SpeakAsyncCancelAll();
         foreach (Slots s in missing)
         {
            RecoManager.Instance.reader.SpeakAsync("I don't know the " + s.ToString());
         }
      }

      protected ActionManager()
      {
         CurrStudent = new Student(DegreeCatalog.Instance.Degrees[0]);
      }
      #region student
      public Student CurrStudent { get; private set; }
      #endregion
   }
}
