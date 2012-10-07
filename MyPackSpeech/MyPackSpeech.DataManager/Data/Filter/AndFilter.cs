﻿using System;
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
   }
}