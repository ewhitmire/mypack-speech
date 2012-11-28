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
      //private Semester startSemester = Semester.Fall;
      public SemesterView()
      {
         InitializeComponent();
         InitGrids();
         //Loaded += SemesterView_Loaded;
      }

      private void InitGrids()
      {
         int startYear = ActionManager.Instance.GradYear - 4;
         sem11.SetSemester(Semester.Fall, startYear);
         sem12.SetSemester(Semester.Spring, startYear + 1);
         sem21.SetSemester(Semester.Fall, startYear + 1);
         sem22.SetSemester(Semester.Spring, startYear + 2);
         sem31.SetSemester(Semester.Fall, startYear + 2);
         sem32.SetSemester(Semester.Spring, startYear + 3);
         sem41.SetSemester(Semester.Fall, startYear + 3);
         sem42.SetSemester(Semester.Spring, startYear + 4);
      }
   }
}
