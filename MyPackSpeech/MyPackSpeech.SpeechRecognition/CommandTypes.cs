using MyPackSpeech.SpeechRecognition.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition
{
   public static class Extensions
   {
      public static Type ActionClass(this CommandTypes command)
      {
         switch (command)
         {
            case CommandTypes.Add:
               return typeof(AddAction);
            case CommandTypes.Remove:
               //return typeof(RemoveAction);
            case CommandTypes.Move:
               //return typeof(MoveAction);
            case CommandTypes.Undo:
               //return typeof(UndoAction);
            case CommandTypes.Show:
               //return typeof(ShowAction);
            default:
               return typeof(IAction);
         }
      }
   }
   public enum CommandTypes
   {
      Add=0,
      Remove,
      Move,
      Swap,
      Undo,
      Show
   }
}
