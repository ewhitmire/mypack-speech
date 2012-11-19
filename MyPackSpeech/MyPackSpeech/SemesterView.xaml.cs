using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
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
using MyPackSpeech.DataManager;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for SemesterView.xaml
   /// </summary>
   public partial class SemesterView : UserControl
   {
      private int startYear = 2012;
      //private Semester startSemester = Semester.Fall;
      public SemesterView()
      {
         InitializeComponent();
         InitGrids();
      }
      private void InitGrids()
      {
         sem11.SetSemester(Semester.Spring, startYear);
         sem12.SetSemester(Semester.Fall, startYear);
         sem21.SetSemester(Semester.Spring, startYear + 1);
         sem22.SetSemester(Semester.Fall, startYear + 1);
         sem31.SetSemester(Semester.Spring, startYear + 2);
         sem32.SetSemester(Semester.Fall, startYear + 2);
         sem41.SetSemester(Semester.Spring, startYear + 3);
         sem42.SetSemester(Semester.Fall, startYear + 3);

         year1.Content = startYear;
         year2.Content = startYear + 1;
         year3.Content = startYear + 2;
         year4.Content = startYear + 3;
      }
   }
}
