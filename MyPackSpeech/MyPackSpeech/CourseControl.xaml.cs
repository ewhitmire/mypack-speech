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
      private BindingList<Course> courses = new BindingList<Course>();
      public BindingList<Course> Courses
      {
         get { return courses; }
      }

      public CourseControl()
      {
         InitializeComponent();

         setDefaultCourses();
      }

      private void setDefaultCourses()
      {
         Courses.Clear();

         Courses.Add(new Course(new Department("Comp Sci", "CSC"), 101, "intro 101"));
         Courses.Add(new Course(new Department("Comp Sci", "CSC"), 201, "intermediate 201"));
         Courses[1].Prerequisites.Add(new CourseFilter(){Dept=Courses[0].Dept, Number=Courses[0].Number, Op = Operator.EQ});

         Courses.Add(new Course(new Department("Comp Sci", "CSC"), 301, "advanced 301"));
         Courses[2].Prerequisites.Add(new CourseFilter() { Dept = Courses[0].Dept, Number = Courses[0].Number, Op = Operator.EQ });
         Courses[2].Prerequisites.Add(new CourseFilter() { Dept = Courses[1].Dept, Number = Courses[1].Number, Op = Operator.EQ });

         Courses.Add(new Course(new Department("Mathematics", "MA"), 141, "Calc one"));
         Courses.Add(new Course(new Department("Mathematics", "MA"), 241, "Calc two"));
         Courses.Add(new Course(new Department("Mathematics", "MA"), 242, "Calc three"));

         courseGrid.AutoGenerateColumns = true;
         courseGrid.ItemsSource = Courses;
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
