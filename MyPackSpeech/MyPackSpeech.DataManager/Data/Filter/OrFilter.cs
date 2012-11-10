using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data.Filter
{
   public class OrFilter<T> : LogicalFilterBase<T>
   {
      public OrFilter(IFilter<T> lhs, IFilter<T> rhs)
         : base(lhs, rhs)
      {
      }

      public override bool Matches(T item)
      {
         if (LHS == null || RHS == null)
         {
            System.Console.WriteLine("yo");
         }
         return LHS.Matches(item) || RHS.Matches(item);
      }

      public override string ToString()
      {
         return String.Format("{0} || {1}", LHS, RHS);
      }
   }
}
