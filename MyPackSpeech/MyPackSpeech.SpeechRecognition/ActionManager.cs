using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Diagnostics;
using MyPackSpeech.SpeechRecognition;

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
         switch (cmd)
         {
            case CommandTypes.Add:
               Debug.Write("Add command");
               break;
         }
      }
   }
}
