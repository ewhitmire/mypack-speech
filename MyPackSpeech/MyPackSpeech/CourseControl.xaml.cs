using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using MyPackSpeech.DataManager;
using MyPackSpeech.DataManager.Data;
using MyPackSpeech.DataManager.Data.Filter;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for UserControl1.xaml
   /// </summary>
   public partial class CourseControl : UserControl
   {
      public CourseControl()
      {
         InitializeComponent();
         if (DesignerProperties.GetIsInDesignMode(this) == false)
            CourseCatalog.Instance.FilterChanged += Instance_FilterChanged;
      }

      void Instance_FilterChanged(object sender, EventArgs e)
      {
         displayCourses();
      }

      private void displayCourses()
      {
         if (!Dispatcher.CheckAccess())
         {
            courseGrid.Invoke(() => displayCourses());
         }

         courseGrid.ItemsSource = CourseCatalog.Instance.FilteredCourses;
      }      
   }
}
