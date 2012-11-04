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
      private event EventHandler<ActionDetectedEventArgs> actionDetected;
      private event EventHandler semesterChanged;

      public event EventHandler<ActionDetectedEventArgs> ActionDetected { add { actionDetected += value; } remove { actionDetected -= value; } }
      public event EventHandler SemesterChanged { add { semesterChanged += value; } remove { semesterChanged -= value; } }

      Stack<IAction> actionHistory = new Stack<IAction>();
      public Semester? CurrentSemester { get; private set; }
      public int? CurrentYear { get; private set; }

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

         switch (cmd)
         {
            case CommandTypes.Add:
            case CommandTypes.Remove:
            case CommandTypes.Move:
               callEvent = doCourseRegistrationAction(result, cmd);
               break;
            case CommandTypes.Undo:
               callEvent = doUndoComnmand(callEvent);
               break;
            case CommandTypes.SetSemester:
               setSemester(result);
               break;
            case CommandTypes.Swap:
            case CommandTypes.Show:
            default:
               throw new ArgumentException("Invalid command type: " + cmd);
         }

         if (callEvent && actionDetected != null)
         {
            ActionDetectedEventArgs args = new ActionDetectedEventArgs(cmd, CurrStudent);
            actionDetected(this, args);
            RecoManager.Instance.reader.SpeakAsync("Ok");
         }
      }

      private void setSemester(RecognitionResult result)
      {
         Semester? semester = CourseConstructor.GetSemester(result.Semantics);
         if (semester.HasValue)
            CurrentSemester = semester.Value;
         int? year = CourseConstructor.GetYear(result.Semantics);
         if (year.HasValue)
            CurrentYear = year.Value;

         if (semester.HasValue || year.HasValue)
            OnSemesterChanged();
      }

      private void OnSemesterChanged()
      {
         EventHandler evt = semesterChanged;
         if (evt != null)
            evt(this, EventArgs.Empty);
      }

      private bool doCourseRegistrationAction(RecognitionResult result, CommandTypes cmd)
      {
         bool callEvent = false;
         IAction action = cmd.GetAction();
         if (action!=null)
         {
            action.Inform(result.Semantics, CurrStudent);
            callEvent = action.Perform();
            //don't push events that didn't work
            if (callEvent)
               actionHistory.Push(action);
         }
         return callEvent;
      }

      private bool doUndoComnmand(bool callEvent)
      {
         if (actionHistory.Count > 0)
         {
            IAction action = actionHistory.Pop();
            action.Undo();
            callEvent = true;
         }
         return callEvent;
      }

      public void PromptForMissing(SemanticValue context, List<Slots> missing)
      {
         RecoManager.Instance.reader.SpeakAsyncCancelAll();
         foreach (Slots s in missing)
         {
            RecoManager.Instance.reader.SpeakAsync("I don't know the " + s.ToString());
         }
      }

      public void notOffered(SemanticValue context, Semester semester) {
         //RecoManager.Instance.reader.SpeakAsyncCancelAll();

         RecoManager.Instance.reader.SpeakAsync("That class is not available in the " + semester.ToString());
      }

      protected ActionManager()
      {
         CurrStudent = new Student(DegreeCatalog.Instance.Degrees.FirstOrDefault());
      }
      #region student
      public Student CurrStudent { get; private set; }
      #endregion
   }
}
