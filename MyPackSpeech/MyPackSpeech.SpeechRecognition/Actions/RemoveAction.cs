using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class RemoveAction : BaseAction
   {
		public ScheduledCourse Course { get; private set; }

      override public bool Perform()
      {
         List<Slots> missing = CourseConstructor.ContainsScheduledCourseData(semantics);


			if (missing.Count > 0)
			{
				PromptForMissing(semantics, missing);
				return false;
			}

         ScheduledCourse sCourse= CourseConstructor.ContructScheduledCourse(semantics);
         Course = Student.Schedule.Courses.Where(c => c.Equals(sCourse)).FirstOrDefault();
		   if (Course != null)
		   {
			   Student.RemoveCourse(Course);
            return true;
		   }
         return false;
      }

      override protected void PromptForMissing(SemanticValueDict semantics, List<Slots> missing)
      {
         
      }


      override public void Undo()
		{
			if (Course != null)
			{
				Student.AddCourse(Course);
			}
		}
	}
}
