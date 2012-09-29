using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public enum Semester
   {
      Fall,
      Spring,
      Summer
   }

   public enum Operator
   {
      /// <summary>
      /// Less than
      /// </summary>
      [Description("<")]
      LT,
      /// <summary>
      /// Less then or equals
      /// </summary>
      [Description("<=")]
      LTE,
      /// <summary>
      /// equals
      /// </summary>
      [Description("==")]
      EQ,
      /// <summary>
      /// not equals
      /// </summary>
      [Description("!=")]
      NEQ,
      /// <summary>
      /// greater than or equals
      /// </summary>
      [Description(">=")]
      GTE,
      /// <summary>
      /// greater than
      /// </summary>
      [Description(">")]
      GT
   }
}
