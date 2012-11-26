using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MyPackSpeech.DataManager.Data;
using MyPackSpeech.DataManager.Data.Filter;

namespace MyPackSpeech.SpeechRecognition
{
   public class MissingPrereqArgs : EventArgs
   {
      private readonly List<IFilter<Course>> prereqs;
      public readonly ScheduledCourse Course;
      public ReadOnlyCollection<IFilter<Course>> Prereqs { get { return prereqs.AsReadOnly(); } }

      public MissingPrereqArgs(ScheduledCourse course, List<IFilter<Course>> prereqs)
      {
         this.prereqs = course.Course.GetAllPrereqs().Distinct().ToList();
         this.prereqs.Sort();

         List<IFilter<Course>> missing = new List<IFilter<Course>>();
         //TODO: BrianR get prereqs using course requirements and registered courses
         foreach (var prereq in prereqs)
         {
            int count = (from c in ActionManager.Instance.CurrStudent.Schedule.Courses
                         where prereq.Matches(c.Course)
                         select c.Course).Count();
            if (count == 0)
            {
               missing.Add(prereq);
            }
         }

         this.prereqs = missing;
         this.Course = course;
      }
   }
}
