using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class AddAction : IAction
   {
      public Student Student { get; private set; }
      public ScheduledCourse Course { get; private set; }
      private SemanticValue semantics = null;
      public void Inform(SemanticValue sem, Student student)
      {
         Student = student;
         semantics = sem;
      }
      public bool Perform()
      {
         List<Slots> missing = CourseConstructor.ValidateCourse(semantics);

         if (missing.Count > 0)
         {
            ActionManager.Instance.PromptForMissing(semantics, missing);
            return false;
         }

         Course = CourseConstructor.ContructScheduledCourse(semantics);
         if (Course != null)
         {
            Student.AddCourse(Course);
            return true;
         }
         return false;
      }

      public void Undo()
      {
         if (Course != null)
            Student.Schedule.Courses.Remove(Course);
      }
   }
}
