using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class DegreeRequirement
   {
      private ScheduledCourse fulfillment;
      public CourseFilter CourseRequirement { get; set; }      

      public DegreeRequirement()
      {
      }

      public ScheduledCourse Fulfillment
      {
         get { return fulfillment; }
         set
         {
            if (fulfillment != value)
            {
               //clear up old references
               if (fulfillment != null && fulfillment.Requirement == this)
                  fulfillment.Requirement = null;

               fulfillment = value;

               //complete link
               if (fulfillment != null)
                  fulfillment.Requirement = this;
            }
         }
      }

      public override string ToString()
      {
         if (Fulfillment != null)
            return string.Format("{0} => {1}", CourseRequirement, Fulfillment);

         return CourseRequirement.ToString();
      }
   }
}
