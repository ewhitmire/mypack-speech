using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data.Filter
{
   public class CourseFilter: Filter<Course>
   {
      public static readonly string DeptNameProperty = "DeptName";
      public static readonly string DeptAbvProperty = "DeptAbv";
      public static readonly string CourseNumberProperty = "Number";

      protected CourseFilter(string propertyName, Operator op, string criteria)
         : base(propertyName, op, criteria)
      { }

      protected CourseFilter(string propertyName, Operator op, int criteria)
         : base(propertyName, op, criteria)
      { }

      public static Filter<Course> DeptName(Operator op, string criteria)
      {
         return new CourseFilter(DeptNameProperty, op, criteria);
      }

      public static Filter<Course> DeptAbv(Operator op, string criteria)
      {
         return new CourseFilter(DeptAbvProperty, op, criteria);
      }

      public static Filter<Course> Number(Operator op, int criteria)
      {
         return new CourseFilter(CourseNumberProperty, op, criteria);
      }

      public static Filter<Course> DeptName(string criteria)
      {
         return new CourseFilter(DeptNameProperty, Operator.EQ, criteria);
      }

      public static Filter<Course> DeptAbv(string criteria)
      {
         return new CourseFilter(DeptAbvProperty, Operator.EQ, criteria);
      }

      public static Filter<Course> Number(int criteria)
      {
         return new CourseFilter(CourseNumberProperty, Operator.EQ, criteria);
      }
   }
}

