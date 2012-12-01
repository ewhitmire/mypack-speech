using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition
{
   public enum Slots
   {
      Department = 0,
      Number,
      Semester,
      Year,
      CourseAnaphora,
      Requirement,
      CourseName,
      ViewName,
      Command,
      Major,
      GradYear,
      YesNo,
      KeyWords,
      KeyWords2,
      KeyWords3,
      KeyWords4
   }
   public enum Views
   {
      Requirements,
      Semester
   }
}
