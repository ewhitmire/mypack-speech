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
using Microsoft.Win32;
using MyPackSpeech.SpeechRecognition;
using MyPackSpeech.DataManager;
namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {

      private CourseCatalog catalog;
      private DegreeCatalog degrees;

      public MainWindow()
      {
         InitializeComponent();
         catalog = new CourseCatalog();
         degrees = new DegreeCatalog();
      }

      private void Load_Click(object sender, RoutedEventArgs e)
      {
         loadFile();
      }

      private void loadFile()
      {
         
      }

      private void loadCourses_Click(object sender, RoutedEventArgs e)
      {
         string file = getFile();
      }

      private void loadStudent_Click(object sender, RoutedEventArgs e)
      {
         string file = getFile();
      }

      private string getFile()
      {
         OpenFileDialog dlg = new OpenFileDialog();
         if(dlg.ShowDialog(this).GetValueOrDefault(false))
            return dlg.FileName;
         return string.Empty;
      }

      private void showReqs()
      {
      }


      private void showReqs_Click(object sender, RoutedEventArgs e)
      {
         showReqs();
      }

      private void showCourses_Click(object sender, RoutedEventArgs e)
      {
         showCourses();
      }

      private void showCourses()
      {
         CourseWindow courseWin = new CourseWindow();
         courseWin.Catalog = this.catalog;
         courseWin.Show();         
      }
   }
}
