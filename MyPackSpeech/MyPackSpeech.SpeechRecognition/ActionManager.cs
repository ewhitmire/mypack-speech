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
using MyPackSpeech.DataManager.Data.Filter;

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

      private Stack<IAction> actionHistory = new Stack<IAction>();
      private IAction currentWorkingAction = null;
      public Semester? CurrentSemester { get; private set; }
      public int? CurrentYear { get; private set; }
      public Course? CurrentCourse { get; private set; }

      public void ProcessResult(RecognitionResult result)
      {
         SemanticValueDict semantics = SemanticValueDict.FromSemanticValue(result.Semantics);
         if (semantics.ContainsKey("command"))
         {
            ProcessCommand(semantics);
         }
         else
         {
            ProcessSupplemental(semantics);
         }
      }

      private void ProcessSupplemental(SemanticValueDict semantics)
      {
         SetContext(semantics);
         InformAndPerformCurrentAction(semantics);
      }

      private void ProcessCommand(SemanticValueDict semantics)
      {
         CommandTypes cmd = (CommandTypes)(semantics["command"].Value);
         bool callEvent = false;

         switch (cmd)
         {
            case CommandTypes.Add:
            case CommandTypes.Remove:
            case CommandTypes.Move:
            case CommandTypes.SetSemester:
               callEvent = doCourseRegistrationAction(semantics, cmd);
               break;
            case CommandTypes.Undo:
               callEvent = doUndoComnmand(callEvent);
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
            RecoManager.Instance.Say("Ok");
         }
         SetContext(semantics);
      }

      public void SetContext(SemanticValueDict semantics)
      {
         Semester? semester = CourseConstructor.GetSemester(semantics);
         if (semester.HasValue)
            CurrentSemester = semester.Value;
         int? year = CourseConstructor.GetYear(semantics);
         if (year.HasValue)
            CurrentYear = year.Value;

         if (semester.HasValue || year.HasValue)
            OnSemesterChanged();

         if (CourseConstructor.ContainsCourseData(semantics).Count == 0)
         {
            CurrentSemester = CourseConstructor.ContructCourse(semantics);
         }
      }

      private void OnSemesterChanged()
      {
         EventHandler evt = semesterChanged;
         if (evt != null)
            evt(this, EventArgs.Empty);
      }

      private bool doCourseRegistrationAction(SemanticValueDict semantics, CommandTypes cmd)
      {
         bool callEvent = false;
         IAction action = cmd.GetAction();
         if (currentWorkingAction == null)
         {
            currentWorkingAction = action;
         }
         else if (currentWorkingAction.GetType().Equals(action.GetType()) || currentWorkingAction is UnknownAction)
         {
            action.Inform(currentWorkingAction.Semantics, CurrStudent);
            currentWorkingAction = action;
         }
         callEvent = InformAndPerformCurrentAction(semantics);
         
         return callEvent;
      }
      private bool InformAndPerformCurrentAction(SemanticValueDict semantics)
      {
         bool callEvent = false;
         if (currentWorkingAction == null)
         {
            currentWorkingAction = new UnknownAction();
         }
         callEvent = currentWorkingAction.Inform(semantics, CurrStudent);
         if (callEvent)
         {
            callEvent = currentWorkingAction.Perform();
            //don't push events that didn't work
            if (callEvent)
            {
               actionHistory.Push(currentWorkingAction);
               currentWorkingAction.GiveConfirmation();
               currentWorkingAction = null;
            }
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

      public void notOffered(SemanticValueDict context, Semester semester)
      {
         //RecoManager.Instance.SayCancelAll();

         RecoManager.Instance.Say("That class is not available in the " + semester.ToString());
      }

      protected ActionManager()
      {
         CurrStudent = new Student(DegreeCatalog.Instance.Degrees.FirstOrDefault());
      }
      #region student
      public Student CurrStudent { get; private set; }
      #endregion

      public void PromptForPreReqs(List<IFilter<Course>> missing)
      {
		  string message = string.Join(",", missing.Select(m=>m.ToString()).ToArray());
		  System.Windows.MessageBox.Show(message);
      }
   }
}
