using System;
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
using System.Windows.Shapes;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for HelpWindow.xaml
   /// </summary>
   public partial class HelpWindow : Window
   {
      public HelpWindow()
      {
         InitializeComponent();
         DialogManager.Instance.OnCloseCommand += ActionManager_OnCloseCommand;
      }

      private void ActionManager_OnCloseCommand(object sender, EventArgs e)
      {
         this.Close();
      }

      private void Button_Click_1(object sender, RoutedEventArgs e)
      {
         this.Close();
      }

   }
}
