﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for BlinkBox.xaml
   /// </summary>
   public partial class BlinkBox : UserControl
   {
      public BlinkBox()
      {
         InitializeComponent();
      }

      public void SetText(String text)
      {
         contentBox.Text = text;
      }
   }
}
