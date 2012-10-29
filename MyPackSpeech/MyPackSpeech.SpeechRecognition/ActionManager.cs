﻿using System;
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

      // delegate declaration 
      public delegate void ActionDetectedHandler(object sender, ActionDetectedEventArgs args);
      // event declaration 
      public event ActionDetectedHandler ActionDetected;

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
         ActionDetectedEventArgs args = new ActionDetectedEventArgs(cmd);
         if (ActionDetected != null)
         {
            ActionDetected(this, args);
         }
         Type ActionType = cmd.ActionClass();
         IAction action = (IAction)Activator.CreateInstance(ActionType);
         action.Inform(result.Semantics);
         action.Perform();            
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
