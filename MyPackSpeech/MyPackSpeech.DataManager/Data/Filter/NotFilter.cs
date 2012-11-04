using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data.Filter
{
   public class NotFilter<T> : IFilter<T>
   {
      public IFilter<T> Criteria { get; private set; }

      public NotFilter(IFilter<T> filter)
      {
         Criteria = filter;
      }
      
      public bool Matches(T item)
      {
         return !Criteria.Matches(item);
      }

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

      public override string ToString()
      {
         return String.Format("!({0})", Criteria);
      }
   }
}
