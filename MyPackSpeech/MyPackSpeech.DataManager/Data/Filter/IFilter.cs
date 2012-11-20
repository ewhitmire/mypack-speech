using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data.Filter
{
   public interface IFilter<T> : IEqualityComparer<IFilter<T>>, IComparable<IFilter<T>>
   {
      bool Matches(T item);
      IFilter<T> And(IFilter<T> rhs);
      IFilter<T> Or(IFilter<T> rhs);
      IFilter<T> Not();
   }
}
