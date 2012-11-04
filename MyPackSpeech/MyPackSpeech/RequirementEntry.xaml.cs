using MyPackSpeech.DataManager.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for RequirementEntry.xaml
   /// </summary>
   public partial class RequirementEntry : UserControl
   {
      private DegreeRequirement req;

      public RequirementEntry()
      {
         InitializeComponent();
      }

      public RequirementEntry(DataManager.Data.DegreeRequirement req)
      {
         InitializeComponent();
         if (req != null)
         {
            this.req = req;
            this.reqName.Content = req.ToString();
         }
      }
   }
}
