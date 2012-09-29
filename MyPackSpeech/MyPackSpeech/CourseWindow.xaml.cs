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
using MyPackSpeech.DataManager;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for CourseWindow.xaml
   /// </summary>
   public partial class CourseWindow : Window
   {
      CourseCatalog catalog;
      public CourseCatalog Catalog
      {
         get { return this.catalog; }
         set
         {
            this.catalog = value;
            updateCourses();
         }
      }

      public CourseWindow()
      {
         InitializeComponent();
      }

      private void updateCourses()
      {
         courseControl.Courses.Clear();
         if (catalog != null)
         {
            foreach (var course in ApplyCourseFilter())
               courseControl.Courses.Add(course);
         }
      }

      private IEnumerable<DataManager.Data.Course> ApplyCourseFilter()
      {
         return catalog.Courses.Where(c => c.Dept.Abv.ToLower() == "ma");
      }
   }
}
