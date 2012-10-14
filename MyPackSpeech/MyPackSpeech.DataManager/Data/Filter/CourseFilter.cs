using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data.Filter
{
   public class CourseFilter: Filter<Course>, IFilter<Course>
   {
      public static readonly string DeptNameProperty = "DeptName";
      public static readonly string DeptAbvProperty = "DeptAbv";
      public static readonly string CourseNumberProperty = "Number";
      public static readonly string CourseNameProperty = "Name";

      protected CourseFilter(string propertyName, string criteria, Operator op)
         : base(propertyName, op, criteria)
      { }

      protected CourseFilter(string propertyName, int criteria, Operator op)
         : base(propertyName, op, criteria)
      { }


      public static Filter<Course> DeptName(string criteria)
      {
         return new CourseFilter(DeptNameProperty, criteria, Operator.EQ);
      }

      public static Filter<Course> DeptName(string criteria, Operator op)
      {
         return new CourseFilter(DeptNameProperty, criteria, op);
      }

      public static Filter<Course> DeptAbv(string criteria, Operator op)
      {
         return new CourseFilter(DeptAbvProperty, criteria, op);
      }

      public static Filter<Course> DeptAbv(string criteria)
      {
         return new CourseFilter(DeptAbvProperty, criteria, Operator.EQ);
      }

      public static Filter<Course> Number(int criteria)
      {
         return new CourseFilter(CourseNumberProperty, criteria, Operator.EQ);
      }

      public static Filter<Course> Number(int criteria, Operator op)
      {
         return new CourseFilter(CourseNumberProperty, criteria, op);
      }

      public static Filter<Course> Name(string criteria)
      {
         return new CourseFilter(CourseNameProperty, criteria, Operator.EQ);
      }

      public static Filter<Course> Name(string criteria, Operator op)
      {
         return new CourseFilter(CourseNameProperty, criteria, op);
      }
   }
}

