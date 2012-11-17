using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data.Filter
{
   public class AndFilter<T> : LogicalFilterBase<T>
   {
      public AndFilter(IFilter<T> lhs, IFilter<T> rhs)
         : base(lhs, rhs)
      {
      }

      public override bool Matches(T item)
      {
         return LHS.Matches(item) && RHS.Matches(item);
      }

      public override string ToString()
      {
         Filter<T> lhs = LHS as Filter<T>;
         Filter<T> rhs = RHS as Filter<T>;
         if (lhs != null && rhs != null)
         {
            //special print for dept abv and course number
            if (lhs.PropertyName == CourseFilter.DeptAbvProperty && rhs.PropertyName == CourseFilter.CourseNumberProperty)
            {
               if (lhs.Op == Operator.EQ && rhs.Op == Operator.EQ)
               {
                  return string.Format("{0} {1}", lhs.StrCriteria, rhs.IntCriteria);
               }
            }
         }
         return String.Format("({0}) && ({1})", LHS, RHS);
      }
   }
}