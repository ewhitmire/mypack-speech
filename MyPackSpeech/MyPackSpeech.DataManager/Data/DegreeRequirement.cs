using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class DegreeRequirement
   {
      private ScheduledCourse fullFillment;
      public CourseFilter CourseRequirement { get; set; }      

      public DegreeRequirement()
      {
      }

      public ScheduledCourse FullFillment
      {
         get { return fullFillment; }
         set
         {
            if (fullFillment != value)
            {
               //clear up old references
               if (fullFillment != null && fullFillment.Requirement == this)
                  fullFillment.Requirement = null;

               fullFillment = value;

               //complete link
               if (fullFillment != null)
                  fullFillment.Requirement = this;
            }
         }
      }

      public override string ToString()
      {
         if (FullFillment != null)
            return string.Format("{0} => {1}", CourseRequirement, FullFillment);

         return CourseRequirement.ToString();
      }
   }
}
