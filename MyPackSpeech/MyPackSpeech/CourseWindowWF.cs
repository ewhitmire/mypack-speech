using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyPackSpeech.DataManager.Data;

namespace MyPackSpeech
{
   public partial class CourseWindowWF : Form
   {
      ObservableCollection<Course> courses;
      public ObservableCollection<Course> Courses
      {
         get { return courses; }
         set         
         {
            courses = value ?? new ObservableCollection<Course>();
            setupGrid();
         }

      }

      public CourseWindowWF()
      {
         InitializeComponent();
      }

      private void setupGrid()
      {
         if (InvokeRequired)
         {
            Invoke((MethodInvoker)setupGrid);
            return;
         }

         BindingSource source = new BindingSource();
         source.DataSource = ApplyFilter(courses);
         this.dataGridView1.DataSource = source;
      }

      private IEnumerable<Course> ApplyFilter(ObservableCollection<Course> courses)
      {
         return courses.Where(c => c.Dept.Abv.ToLower() == "csc");
      }
      
   }
}
