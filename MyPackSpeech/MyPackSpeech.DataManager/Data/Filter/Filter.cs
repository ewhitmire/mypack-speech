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
         return new OrFilter<T>(this, rhs);
      }

      public IFilter<T> Not()
      {
         return new NotFilter<T>(this);
      }
   }
}