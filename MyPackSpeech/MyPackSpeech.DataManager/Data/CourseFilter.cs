using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class CourseFilter
   {
      public Department Dept { get; set; }
      public int Number { get; set; }
      public Operator Op { get; set; }

      public CourseFilter()
      {
      }

      public bool Matches(Course course)
      {
         if (course.Dept != Dept)
            return false;

         return compareNumber(course);
      }

      private bool compareNumber(Course course)
      {
         switch (Op)
         {
            case Operator.LT:
               return course.Number < Number;
            case Operator.LTE:
               return course.Number <= Number;
            case Operator.E:
               return course.Number == Number;
            case Operator.NE:
               return course.Number != Number;
            case Operator.GTE:
               return course.Number >= Number;
            case Operator.GT:
               return course.Number > Number;
            default:
               throw new ArgumentException("Invalid operator " + Op);
         }
      }

      public override string ToString()
      {
         return string.Format("{0} {1}{2}", getOpString(), Dept, Number);
      }

      private string getOpString()
      {
         return Utils.GetDescription(typeof(Operator), Op.ToString());
      }
   }
}
