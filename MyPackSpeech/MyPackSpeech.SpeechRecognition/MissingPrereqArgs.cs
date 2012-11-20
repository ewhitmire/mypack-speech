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
      public readonly Course Course;
      public ReadOnlyCollection<IFilter<Course>> Prereqs { get { return prereqs.AsReadOnly(); } }

      public MissingPrereqArgs(Course course, List<IFilter<Course>> prereqs)
      {
         this.prereqs = course.GetAllPrereqs().Distinct().ToList();
         this.Course = course;
      }
   }
}
