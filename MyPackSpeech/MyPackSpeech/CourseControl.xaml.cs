using System;
using System.Collections.Generic;
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
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for UserControl1.xaml
   /// </summary>
   public partial class CourseControl : UserControl
   {
      private BindingList<ICourse> courses = new BindingList<ICourse>();
      public BindingList<ICourse> Courses
      {
         get { return courses; }
      }

      public CourseControl()
      {
         InitializeComponent();

         courseGrid.ItemsSource = courses;
      }

      private void displayCourses()
      {
         if (!Dispatcher.CheckAccess())
         {
            courseGrid.Invoke(() => displayCourses());
         }
      }      
   }
}
