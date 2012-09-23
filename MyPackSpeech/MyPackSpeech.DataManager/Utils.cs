using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager
{
   public static class Utils
   {
      /// <summary>
      /// Gets the description attribute from an enum
      /// </summary>
      /// <param name="type"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      public static string GetDescription(Type type, string value)
      {
         return ((DescriptionAttribute)(type.GetMember(value)[0].GetCustomAttributes(typeof(DescriptionAttribute), false)[0])).Description;
      }
   }
}
