using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MyPackSpeech
{
   public static class UIUtils
   {
      public static void Invoke(this Control Control, Action Action)
      {
         Control.Dispatcher.Invoke(Action);
      }
   }
}
