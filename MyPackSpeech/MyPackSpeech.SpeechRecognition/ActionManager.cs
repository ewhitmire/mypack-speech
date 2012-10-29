using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Diagnostics;
using MyPackSpeech.SpeechRecognition;
using MyPackSpeech.SpeechRecognition.Actions;

namespace MyPackSpeech
{
   public class ActionDetectedEventArgs : System.EventArgs
   {
      public CommandTypes type;

      public ActionDetectedEventArgs(CommandTypes t)
      {
         this.type = t;
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
		  if (ActionDetected != null)
		  {
			  ActionDetectedEventArgs args = new ActionDetectedEventArgs(cmd);
			  ActionDetected(this, args);
		  }
        if (cmd == CommandTypes.Undo)
        {
           if (actionHistory.Count > 0)
           {
              IAction action = actionHistory.Pop();
              action.Undo();
           }
        }
        else
        {
           Type ActionType = cmd.ActionClass();
           IAction action = (IAction)Activator.CreateInstance(ActionType);
           action.Inform(result.Semantics);
           action.Perform();
           actionHistory.Push(action);
        }
	  }

      public static List<Slots> ValidateExistingCourse(SemanticValue course)
      {
         List<Slots> missing = new List<Slots>();
         if (!course.ContainsKey(Slots.Department.ToString()))
         {
            missing.Add(Slots.Department);
         }
         if (!course.ContainsKey(Slots.Number.ToString()))
         {
            missing.Add(Slots.Department);
         }
         return missing;
      }

      public static List<Slots> ValidateCourse(SemanticValue course)
      {
         List<Slots> missing = new List<Slots>();
         if (!course.ContainsKey(Slots.Department.ToString()))
         {
            missing.Add(Slots.Department);
         }
         if (!course.ContainsKey(Slots.Number.ToString()))
         {
            missing.Add(Slots.Department);
         }
         if (!course.ContainsKey(Slots.Semester.ToString()))
         {
            missing.Add(Slots.Semester);
         }
         if (!course.ContainsKey(Slots.Year.ToString()))
         {
            missing.Add(Slots.Year);
         }
         return missing;
      }
      public void PromptForMissing(SemanticValue context, List<Slots> missing)
      {
         Debug.Write("hooray!");
      }
   }
}
