using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class CourseFilter
   {
      private static Dictionary<Operator, Func<int, int, bool>> OperatorFuncs = new Dictionary<Operator, Func<int, int, bool>>();
      static CourseFilter()
      {
         OperatorFuncs[Operator.LT] = (l, r) => l < r;
         OperatorFuncs[Operator.LTE] = (l, r) => l <= r;
         OperatorFuncs[Operator.EQ] = (l, r) => l == r;
         OperatorFuncs[Operator.NEQ] = (l, r) => l != r;
         OperatorFuncs[Operator.GT] = (l, r) => l > r;
         OperatorFuncs[Operator.GTE] = (l, r) => l >= r;
      }

      public Department Dept { get; set; }
      public int Number { get; set; }
      public Operator Op { get; set; }
      public List<CourseFilter> And { get; set; }
      public List<CourseFilter> Or { get; set; }
      public List<CourseFilter> Not { get; set; }

      public CourseFilter()
      {
         And = new List<CourseFilter>();
         Or = new List<CourseFilter>();
         Not = new List<CourseFilter>();
      }

      public bool Matches(Course course)
      {
         bool fMatches = false;
         if (course.Dept == Dept)
         {
            fMatches = compareNumber(course);
         }

         if (!fMatches)
         {
            foreach (CourseFilter filter in Or)
            {
               if (filter.Matches(course))
               {
                  fMatches = true;
                  break;
               }
            }
         }

         if (fMatches)
         {
            foreach (CourseFilter filter in Not)
            {
               if (filter.Matches(course))
               {
                  fMatches = false;
                  break;
               }
            }
         }

         if (fMatches)
         {
            foreach (CourseFilter filter in And)
            {
               if (!filter.Matches(course))
               {
                  fMatches = false;
                  break;
               }
            }

         }
         return fMatches;
      }

      private bool compareNumber(Course course)
      {
         return OperatorFuncs[Op](course.Number, Number);
      }

      public override string ToString()
      {
         String text = "[ ";
         if (Number > 0)
         {
            text += string.Format("{0} {1}{2}", getOpString(), Dept, Number);
         }

         if (Or.Count > 0)
         {
            text += "OR: (" + String.Join(" | ", Or) +")";
         }
         if (And.Count > 0)
         {
            text += "AND: (" + String.Join(" & ", And) +")";
         }
         if (Not.Count > 0)
         {
            text += " NOT: (" + String.Join(", ", Not) +")";
         }

         text += " ]";

         return text;
      }

      private string getOpString()
      {
         return Utils.GetDescription(typeof(Operator), Op.ToString());
      }
   }
}
