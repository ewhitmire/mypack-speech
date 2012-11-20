using MyPackSpeech.DataManager;
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
            IEnumerable<Course> courseList = CourseCatalog.Instance.GetCourses(req.CourseRequirement);
            this.courses.Content = CourseCatalog.FormatCourseList(courseList);
            updateContent();

         }
         ActionManager.Instance.CurrStudent.ScheduleChanged += Student_ScheduleChanged;
         ActionManager.Instance.CurrStudent.BookmarksChanged += Student_BookmarksChanged;
      }
      void Student_BookmarksChanged(object sender, EventArgs e){
         updateContent();
      }
      void Student_ScheduleChanged(object sender, EventArgs e)
      {
         updateContent();
      }

      private void updateContent()
      {

         if (req != null)
         {
            this.reqName.Content = req.Name;
            if (req.Fulfillment != null)
            {
               this.fulfillment.Content = req.Fulfillment.ToString();
               this.fulfillment.Foreground = Brushes.Green;
            }
            else
            {
               this.fulfillment.Content = "Not fulfilled";
               this.fulfillment.Foreground = Brushes.Red;
            }
         }
      }
   }
}
