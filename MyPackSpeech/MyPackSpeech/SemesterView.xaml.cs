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
      private List<SemesterControl> semesters;
      private int startYear = 2012;
      private Semester startSemester = Semester.Fall;
      public SemesterView()
      {
         InitializeComponent();
         InitGrids();
      }
      private void InitGrids()
      {
         semesters = new List<SemesterControl>();
         semesters.Add(sem11);
         semesters.Add(sem12);
         semesters.Add(sem21);
         semesters.Add(sem22);
         semesters.Add(sem31);
         semesters.Add(sem32);
         semesters.Add(sem41);
         semesters.Add(sem42);
         for (int i = 0; i < 4; i++)
         {
            semesters[2 * i].SetSemester(Semester.Fall, startYear + i);
            semesters[2 * i + 1].SetSemester(Semester.Spring, startYear + i);
         }
      }

      
   }
}
