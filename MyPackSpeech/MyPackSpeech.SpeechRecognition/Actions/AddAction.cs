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
         List<Course> missingClasses = Student.Schedule.GetMissingPreReqs(Course);

         if (missingClasses.Count > 0)
         {
            ActionManager.Instance.PromptForPreReqs(missingClasses);
            return false;
         }
         if (Course != null)
         {
            switch (Course.Semester) { 
               case Semester.Fall:
                  if (!Course.Course.fall) {
                     ActionManager.Instance.notOffered(semantics, Course.Semester);
                     return false;
                  }
                  break;
               case Semester.Spring:
                  if (!Course.Course.spring) {
                     ActionManager.Instance.notOffered(semantics, Course.Semester);
                     return false;
                  }
                  break;
               default:
                  return false;

            }

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
