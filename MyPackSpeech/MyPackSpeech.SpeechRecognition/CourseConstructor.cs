using MyPackSpeech.DataManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;

namespace MyPackSpeech.SpeechRecognition
{
   public static class CourseConstructor
   {
      public static Course ContructCourse(SemanticValue semantics)
      {
         return null;
      }

      public static ScheduledCourse ContructScheduledCourse(SemanticValue semantics)
      {
         Course course = ContructCourse(semantics);
         return null;
      }



   }
}
