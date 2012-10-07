using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data.Filter
{
   public abstract class LogicalFilterBase<T> : IFilter<T>
   {
      public IFilter<T> LHS { get; private set; }
      public IFilter<T> RHS { get; private set; }

      protected LogicalFilterBase(IFilter<T> lhs, IFilter<T> rhs)
      {
         LHS = lhs;
         RHS = rhs;
      }

      public abstract bool Matches(T item);

      public IFilter<T> And(IFilter<T> rhs)
      {
         return new AndFilter<T>(this, rhs);
      }

      public IFilter<T> Or(IFilter<T> rhs)
      {
         return new OrFilter<T>(this, rhs);
      }

      public IFilter<T> Not()
      {
         return new NotFilter<T>(this);
      }
   }
}
