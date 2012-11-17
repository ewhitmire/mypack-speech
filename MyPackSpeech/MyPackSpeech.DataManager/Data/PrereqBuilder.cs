using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPackSpeech.DataManager.Data.Filter;

namespace MyPackSpeech.DataManager.Data
{
   public static class PrereqBuilder
   {
      private const string OR = "or";
      private const string AND = "and";
      public static IFilter<Course>[] GetPreReqFilters(List<string> parsedPreReqs)
      {
         if (parsedPreReqs == null)
         {
            return new IFilter<Course>[0];
         }
         List<IFilter<Course>> filters = new List<IFilter<Course>>();

         IFilter<Course> filter = null;
         string dept = null;

         bool doOr = false;
         bool add = false;
         foreach (string tok in parsedPreReqs)
         {
            int num;
            if (int.TryParse(tok, out num))
            {
               //check if this is part of a combined filter
               filter = CreateFilter(filter, dept, num, doOr);
               //and/or process so reset
               doOr = false;
            }
            else if (isOr(tok))
            {
               doOr = true;
            }
            else if (isAnd(tok))
            {
               add = true;
            }
            else
            {
               dept = tok;
               
               //start of new department check to add new filter or if part of and/or
               add = (!doOr && filter != null);
            }

            if (add)
            {
               if (filter != null)
               {
                  filters.Add(filter);
               }
               filter = null;
               doOr = false;
               dept = null;
               add = false;
            }
         }

         if (filter != null)
            filters.Add(filter);

         return filters.ToArray();
      }

      private static IFilter<Course> CreateFilter(IFilter<Course> filter, string dept, int num, bool doOr)
      {
         IFilter<Course> course = CourseFilter.DeptAbv(dept).And(CourseFilter.Number(num));
         if (doOr)
         {
            filter = filter.Or(course);
         }
         else
         {
            if (filter != null)
               throw new InvalidOperationException("filter cannot be non-null without and/or");
            filter = course;
         }

         return filter;
      }

      private static bool isOr(string tok)
      {
         return tok == OR;
      }

      private static bool isAnd(string tok)
      {
         return tok == AND;
      }
   }
}