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
            case CommandTypes.SetSemester:
               return new SetSemesterAction();
            case CommandTypes.Inquire:
               return new InquireAction();
            case CommandTypes.Bookmark:
               return new BookmarkAction();
            case CommandTypes.View:
               return new ViewAction();
            case CommandTypes.Help:
               return new HelpAction();
            case CommandTypes.Save:
            case CommandTypes.Load:
               return new SaveLoadAction();
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
      SetSemester,
      Inquire,
      Bookmark,
      View,
      Help,
      Save,
      Search,
      Load
   }
}