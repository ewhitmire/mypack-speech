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

   public class ActionManager : IDialogueManager
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
      public Course CurrentCourse { get; private set; }

      public int GradYear = 2016;

      public void ProcessResult(RecognitionResult result)
      {
         SemanticValueDict semantics = SemanticValueDict.FromSemanticValue(result.Semantics);
         if (isJunkSpeech(semantics))
         {
            return;
         }
         if (semantics.ContainsKey(Slots.Command.ToString()))
         {
            ProcessCommand(semantics);
         }
         else
         {
            ProcessSupplemental(semantics);
         }
      }

      private bool isJunkSpeech(SemanticValueDict semantics)
      {
         if (semantics.Count() == 1 && semantics.HasSlot(Slots.CourseAnaphora))
         {
            return true;
         }
         return false;
      }

      private void ProcessSupplemental(SemanticValueDict semantics)
      {
         SetContext(semantics);
         InformAndPerformCurrentAction(semantics);
      }

      private void ProcessCommand(SemanticValueDict semantics)
      {
         CommandTypes cmd = (CommandTypes)(semantics[Slots.Command.ToString()].Value);
         bool callEvent = false;

         SetContext(semantics);

         switch (cmd)
         {
            case CommandTypes.Bookmark:
            case CommandTypes.Add:
            case CommandTypes.Remove:
            case CommandTypes.Move:
            case CommandTypes.SetSemester:
            case CommandTypes.Inquire:
            case CommandTypes.View:
            case CommandTypes.Save:
            case CommandTypes.Load:
               callEvent = doCourseRegistrationAction(semantics, cmd);
               break;
            case CommandTypes.Undo:
               callEvent = doUndoComnmand(callEvent);
               break;
            case CommandTypes.Help:
               IAction action = cmd.GetAction();
               if (action != null)
                  action.Perform();
               beHelpful();
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
      }

      public void SetContext(SemanticValueDict semantics)
      {
         Semester? semester = CourseConstructor.GetSemester(semantics);
         if (semester.HasValue)
            CurrentSemester = semester.Value;

         int? year = CourseConstructor.GetYear(semantics);
         if (year.HasValue)
            CurrentYear = year.Value;

         if (semester.HasValue && !year.HasValue)
         {
            if (semester == Semester.Fall)
            {
               CurrentYear--;
            }
            else
            {
               CurrentYear++;
            }
         }

         if (semester.HasValue || year.HasValue)
            OnSemesterChanged();

         if (CourseConstructor.SemanticsContainsCourseData(semantics).Count == 0)
         {
            CurrentCourse = CourseConstructor.ContructCourse(semantics);
            if (CurrentCourse != null)
            {
               SetInfoPane(CurrentCourse.DeptAbv + CurrentCourse.Number + "\n" + CurrentCourse.Description);
            }
         }
      }

      private void OnSemesterChanged()
      {
         EventHandler evt = semesterChanged;
         if (evt != null)
            evt(this, EventArgs.Empty);
      }

      private void beHelpful() {
         RecoManager.Instance.Say("If you would like to add a class, say something like, I would like to add CSC 591 to my Fall 2012 semester.");
         RecoManager.Instance.Say("If you don't know what you would like to say, press the help button for some suggestions.");
      
      
      }
      private bool doCourseRegistrationAction(SemanticValueDict semantics, CommandTypes cmd)
      {
         bool callEvent = false;
         IAction action = cmd.GetAction();
         if (currentWorkingAction == null || !currentWorkingAction.GetType().Equals(action.GetType()))
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

      public void InformPreReqs(ScheduledCourse course, List<IFilter<Course>> missing)
      {
         OnMissingPreReqs(course, missing);
         List<IFilter<Course>> scheduledTooLate = new List<IFilter<Course>>();
         foreach (var prereq in missing)
         {
            int count = (from c in CurrStudent.Schedule.Courses
                         where prereq.Matches(c.Course)
                         select c.Course).Count();
            if (count > 0)
            {
               scheduledTooLate.Add(prereq);
            }
         }

         if (scheduledTooLate.Count == 0)
         {
            RecoManager.Instance.Say("You are missing prerequisites for " + course.Course.ToString());
         }
         else {
            string str = "";
            foreach (IFilter<Course> filter in scheduledTooLate)
            {
               str += Filter<Course>.toSpokenString(filter) + " ";
            }

            RecoManager.Instance.Say("You must schedule " + course.Course.ToString() + " after " + str); 
         }
      }

      private event EventHandler<MissingPrereqArgs> missingPrereqs;
      public event EventHandler<MissingPrereqArgs> MissingPrereqs { add { missingPrereqs += value; } remove { missingPrereqs -= value; } }

      private void OnMissingPreReqs(ScheduledCourse course, List<IFilter<Course>> missing)
      {
         var evt = missingPrereqs;
         if (evt != null)
            evt(this, new MissingPrereqArgs(course, missing));
      }
      
      private event EventHandler<InfoPaneSetArgs> infoPaneSet;
      public event EventHandler<InfoPaneSetArgs> InfoPaneSet { add { infoPaneSet += value; } remove { infoPaneSet -= value; } }


      public void SetInfoPane(String text)
      {
         var evt = infoPaneSet;
         if (evt != null)
            evt(this, new InfoPaneSetArgs(text));

      }

      private event EventHandler<ViewChangeArgs> onViewChange;
      public event EventHandler<ViewChangeArgs> OnViewChange { add { onViewChange += value; } remove { onViewChange -= value; } }

      public void SwitchView(Views view)
      {
         var evt = onViewChange;
         if (evt != null)
            evt(this, new ViewChangeArgs(view));
      }

      private event EventHandler onShowHelp;
      public event EventHandler OnShowHelp { add { onShowHelp += value; } remove { onShowHelp -= value; } }

      public void ShowHelp()
      {
         var evt = onShowHelp;
         if (evt != null)
            evt(this, EventArgs.Empty);
      }
   }
}