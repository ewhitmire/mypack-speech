﻿using System;
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
      public List<CourseFilter> SubFilters { get; set; }
     
      public CourseFilter()
      {
         SubFilters = new List<CourseFilter>();
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
            foreach (CourseFilter filter in SubFilters)
            {
               fMatches = fMatches | filter.Matches(course);
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
         return string.Format("{0} {1}{2}", getOpString(), Dept, Number);
      }

      private string getOpString()
      {
         return Utils.GetDescription(typeof(Operator), Op.ToString());
      }
   }
}
