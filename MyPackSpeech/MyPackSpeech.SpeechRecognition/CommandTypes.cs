using MyPackSpeech.SpeechRecognition.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition
{
   public static class Extensions
   {
      public static IAction GetAction(this CommandTypes command)
      {
         switch (command)
         {
            case CommandTypes.Add:
               return new AddAction();
            case CommandTypes.Remove:
               return new RemoveAction();
            case CommandTypes.Move:
               return new MoveAction();
            case CommandTypes.Show:
            default:
               break;
         }

         return null;
      }
   }
   public enum CommandTypes
   {
      Add = 0,
      Remove,
      Move,
      Swap,
      Undo,
      Show,
      SetSemester
   }
}