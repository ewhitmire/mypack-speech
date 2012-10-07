using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data.Filter
{
   public static class Comparisons
   {      
      public static Dictionary<Operator, Func<int, int, bool>> IntFuncs = new Dictionary<Operator, Func<int, int, bool>>();
      public static Dictionary<Operator, Func<string, string, bool>> StrFuncs = new Dictionary<Operator, Func<string, string, bool>>();
      
      static Comparisons()
      {
         IntFuncs[Operator.LT] = (l, r) => l < r;
         IntFuncs[Operator.LTE] = (l, r) => l <= r;
         IntFuncs[Operator.EQ] = (l, r) => l == r;
         IntFuncs[Operator.NEQ] = (l, r) => l != r;
         IntFuncs[Operator.GT] = (l, r) => l > r;
         IntFuncs[Operator.GTE] = (l, r) => l >= r;

         StrFuncs[Operator.EQ] = (l, r) => l.Equals(r, StringComparison.CurrentCultureIgnoreCase);
         StrFuncs[Operator.NEQ] = (l, r) => !l.Equals(r, StringComparison.CurrentCultureIgnoreCase);
         StrFuncs[Operator.In] = (l, r) => r.ToLower().Contains(l.ToLower());
      }

      public static bool Eval(int lhs, int rhs, Operator op)
      {
         return IntFuncs[op](lhs, rhs);
      }

      public static bool Eval(string lhs, string rhs, Operator op)
      {
         return StrFuncs[op](lhs, rhs);
      }
   }
}
