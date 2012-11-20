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
         if (rhs == null)
         {
            return this;
         }
         return new OrFilter<T>(this, rhs);
      }

      public IFilter<T> Not()
      {
         return new NotFilter<T>(this);
      }

      public override bool Equals(object obj)
      {
         return Equals(this, obj as LogicalFilterBase<T>);
      }

      public bool Equals(IFilter<T> x, IFilter<T> y)
      {
         LogicalFilterBase<T> xFilter = x as LogicalFilterBase<T>;
         LogicalFilterBase<T> yFilter = y as LogicalFilterBase<T>;

         if (xFilter == null || yFilter == null)
            return false;

         return xFilter.LHS.Equals(yFilter.LHS)
            &&  xFilter.RHS.Equals(yFilter.RHS);
      }

      public int GetHashCode(IFilter<T> obj)
      {
         return obj.GetHashCode();
      }

      public int CompareTo(IFilter<T> other)
      {
         if(!(other is LogicalFilterBase<T>))
            return -1;
         var c1 = CourseCatalog.Instance.GetCourses(this as IFilter<Course>).ToList();
         var c2 = CourseCatalog.Instance.GetCourses(other as IFilter<Course>).ToList();

         int cmp = 0;
         if (c1.Count == c2.Count)
         {
            for (int i = 0; i < c1.Count; i++)
            {
               cmp = c1[i].CompareTo(c2[i]);
               if (cmp != 0)
                  break;
            }
         }

         if (cmp == 0)
         {
            cmp = LHS.CompareTo(other);
            if (cmp == 0)
               cmp = RHS.CompareTo(other);
         }

         return cmp;
      }
   }
}
