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
using MyPackSpeech.DataManager.Data;
using System.Collections.ObjectModel;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      DebugWindow debugWnd;

      List<Course> bookmarked = new List<Course>();

      public MainWindow()
      {
         InitializeComponent();
         Loaded += MainWindow_Loaded;
      }

      void MainWindow_Loaded(object sender, RoutedEventArgs e)
      {
         showDebugWindow();
         ActionManager.Instance.MissingPrereqs += Instance_MissingPrereqs;
         ActionManager.Instance.InfoPaneSet += ActionManager_InfoPaneSet;
      }

      void Instance_MissingPrereqs(object sender, MissingPrereqArgs e)
      {
         infoBox.Text = "Missing Prerequisites for " + e.Course + "\n" + String.Join("\n", e.Prereqs.Select(p => p.ToString()).ToArray());
      }
      void ActionManager_InfoPaneSet(object sender, InfoPaneSetArgs e)
      {
         infoBox.Text = e.Text;
      }
      protected override void OnClosed(EventArgs e)
      {
         closeDebugWindow();
         base.OnClosed(e);
      }

      public void addBookmark(Course course)
      {
         bookmarked.Add(course);
         showBookmarks();
      }

      public void removeBookmark(Course course)
      {

         bookmarked.Remove(course);
         showBookmarks();
      }

      public void showBookmarks()
      {
         String marks = "";

         for (int i = 0; i < bookmarked.Count; i++)
         {
            Course course = bookmarked[i];
            marks += "" + course.Dept.Name + "(" + course.DeptAbv + ")" + " " + course.Number + "\n";
         }

         bookmarks.Text = marks;
      }

      public void showInfo(Course course)
      {
         infoBox.Text = "" + course.Dept.Name + "(" + course.DeptAbv + ")" + " " + course.Number + "\n" +
             course.Description;
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
         Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
         if (dlg.ShowDialog(this).GetValueOrDefault(false))
            return dlg.FileName;
         return string.Empty;
      }

      private void isSpeechOn_Checked(object sender, RoutedEventArgs e)
      {
         RecoManager.Instance.StartSpeechReco();
      }

      private void isSpeechOn_Unchecked(object sender, RoutedEventArgs e)
      {
         RecoManager.Instance.StopSpeechReco();
      }

      #region Debug
      private void closeDebugWindow()
      {
         if (debugWnd != null)
         {
            debugWnd.Close();
         }
      }

      private void showDebugWindow()
      {
         if (debugWnd == null)
         {
            debugWnd = new DebugWindow();
            debugWnd.Show();
         }
      }
      #endregion

      private void semView_Loaded(object sender, RoutedEventArgs e)
      {

      }
   }
}