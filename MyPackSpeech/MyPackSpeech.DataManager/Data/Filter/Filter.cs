using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyPackSpeech.DataManager.Data.Filter
{
   public abstract class Filter<T> : IFilter<T>
   {
      public Operator Op { get; private set; }
      public string StrCriteria { get; private set; }
      public int IntCriteria { get; private set; }
      public PropertyInfo Property { get; private set; }
      public string PropertyName { get; private set; }

      private Filter(string propertyName, Operator op)
      {
         Op = op;
         PropertyName = propertyName;
      }

      public Filter(string propetyName, Operator op, string criteria)
         : this(propetyName, op)
      {
         StrCriteria = criteria;
         setPropertyInfo();
      }

      public Filter(string propetyName, Operator op, int criteria)
         : this(propetyName, op)
      {
         IntCriteria = criteria;
         setPropertyInfo();
      }

      private void setPropertyInfo()
      {
         PropertyInfo info = typeof(T).GetProperty(PropertyName);
         if (info.PropertyType != typeof(string) && info.PropertyType != typeof(int))
            throw new ArgumentException("Only supports strings and ints");

         Property = info;
      }

      public virtual bool Matches(T item)
      {
         object itemProperty = Property.GetValue(item, null);

         if (itemProperty is string)
         {
            return Comparisons.Eval((string)itemProperty, StrCriteria, Op);
         }
         else
         {
            return Comparisons.Eval((int)itemProperty, IntCriteria, Op);
         }
      }

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

      public override string ToString()
      {
         string crit = String.IsNullOrEmpty(StrCriteria) 
            ? IntCriteria.ToString() 
            : StrCriteria;
         return String.Format("{0} {1} {2}", PropertyName, Utils.GetDescription(Op.GetType(), Op.ToString()), crit);
      }

      public override bool Equals(object obj)
      {
         return Equals(this, obj as Filter<T>);
      }

      public bool Equals(IFilter<T> x, IFilter<T> y)
      {
         Filter<T> xFilter = x as Filter<T>;
         Filter<T> yFilter = y as Filter<T>;

         if (xFilter != null && yFilter != null)
         {
            return xFilter.Op == yFilter.Op && xFilter.StrCriteria == yFilter.StrCriteria && xFilter.IntCriteria == yFilter.IntCriteria && xFilter.PropertyName == yFilter.PropertyName;
         }

         return false;
      }

      public int GetHashCode(IFilter<T> obj)
      {
         return base.GetHashCode();
      }

      public static string PrettyString(IFilter<Course> req)
      {
         IEnumerable<Course> courses = CourseCatalog.Instance.GetCourses(req);

         if (courses.Count() == 1)
         {
            Course match = courses.First();
            return match.DeptAbv + match.Number + " - " + match.Name;
         }

         List<Course> orRequirements = courses.ToList();
         string str = "        " + orRequirements[0].DeptAbv + orRequirements[0].Number + " - " + orRequirements[0].Name;
         for (int i = 1; i < orRequirements.Count; i++)
         {
            Course match = orRequirements[i];
            str += "\n    or " + match.DeptAbv + match.Number + " - " + match.Name;
         }
         return str;
      }


      public int CompareTo(IFilter<T> other)
      {
         if (other == null)
            return -1;

         if (other is Filter<T>)
         {
            Filter<T> filter = other as Filter<T>;
            int cmp = Op.CompareTo(filter.Op);
            if (cmp == 0)
            {
               if (StrCriteria != null)
                  cmp = StrCriteria.CompareTo(filter.StrCriteria ?? "");
               if (cmp == 0)
                  cmp = IntCriteria.CompareTo(filter.IntCriteria);
            }

            return cmp;
         }

         if (other is LogicalFilterBase<T>)
         {
            int cmp = this.CompareTo((other as LogicalFilterBase<T>).LHS);
            if(cmp ==0)
               cmp = this.CompareTo((other as LogicalFilterBase<T>).RHS);

            return cmp;
         }

         return 1;
      }
   }
}