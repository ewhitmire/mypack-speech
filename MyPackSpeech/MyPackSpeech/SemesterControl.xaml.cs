using MyPackSpeech.DataManager.Data;
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

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for Semester.xaml
   /// </summary>
   public partial class SemesterControl : UserControl
   {
      private Semester sem;
      private int year;
      public SemesterControl()
      {
         InitializeComponent();
         CollectionViewSource viewSource = (CollectionViewSource)Resources["viewSource"];
         viewSource.Source = ActionManager.Instance.CurrStudent.Schedule.Courses;

      }
      public void SetSemester(Semester sem, int year)
      {
         this.sem = sem;
         this.year = year;
         //this.title.Content = sem.ToString() + " " + year.ToString();
      }
      private void SemesterFilter(object sender, FilterEventArgs e)
      {
         ScheduledCourse course = e.Item as ScheduledCourse;
         e.Accepted = course.Semester.Equals(sem) && course.Year.Equals(year);
      }

      private void data_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
      {
         if (e.PropertyName == "Course")
         {
            e.Column.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            return;
         }
         if (e.PropertyName == "CourseName")
         {
            e.Column.Width = new DataGridLength(5, DataGridLengthUnitType.Star);
            e.Column.Header = "Course Name";
            return;
         }
         if (e.PropertyName == "Credits")
         {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            return;
         }
         e.Cancel = true;
      }

   }
}
