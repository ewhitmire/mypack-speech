﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class SetSemesterAction : BaseAction
   {
      public ScheduledCourse Course { get; private set; }

      override public bool Perform()
      {
         List<Slots> missing = new List<Slots>();//CourseConstructor(semantics);

         if (missing.Count > 0)
         {
            PromptForMissing(Semantics, missing);
            return false;
         }

         if (!isInRange()) {
            return false;
         }

         DialogManager.Instance.SetContext(Semantics);

         RecoManager.Instance.Say("Ok");
         return false;
      }


      private Boolean isInRange()
      {

         Semester currentSemester;
         int currentYear;
         currentSemester = Semester.Summer;
         Semester? semester = CourseConstructor.GetSemester(Semantics);
         if (semester.HasValue)
         {
            currentSemester = semester.Value;
         }
         currentYear = 0;
         int? year = CourseConstructor.GetYear(Semantics);
         if (year.HasValue)
         {
            currentYear = year.Value;
         }

         if (!CourseConstructor.IsSemesterValid(currentSemester, currentYear))
         {
            RecoManager.Instance.Say("That semester is outside of the current range.");
            return false;
         }
         return true;


      }

      override protected void PromptForMissing(SemanticValueDict semantics, List<Slots> missing)
      {
         
      }


      override public void Undo()
      {
         if (Course != null)
            Student.Schedule.Courses.Remove(Course);
      }
   }
}
