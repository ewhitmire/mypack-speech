using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MyPackSpeech.DataManager;
using MyPackSpeech.DataManager.Data;


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
            courseControl.Courses = new ObservableCollection<ICourse>(ApplyCourseFilter());
         }
      }

      private IEnumerable<ICourse> ApplyCourseFilter()
      {
         return catalog.Courses.Where(c => c.Dept.Abv.ToLower() == "csc");
      }


   }
}
