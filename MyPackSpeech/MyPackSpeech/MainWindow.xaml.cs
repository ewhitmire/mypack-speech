using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using MyPackSpeech.DataManager;
using MyPackSpeech.DataManager.Data;
using MyPackSpeech.DataManager.Data.Filter;
using MyPackSpeech.SpeechRecognition;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      DebugWindow debugWnd;
      HelpWindow popUp;
      StartScreen starter;

      public MainWindow()
      {
         InitializeComponent();
         Loaded += MainWindow_Loaded;
      }

      void MainWindow_Loaded(object sender, RoutedEventArgs e)
      {
         showDebugWindow();
         showStartScreen();
         ActionManager.Instance.MissingPrereqs += Instance_MissingPrereqs;
         ActionManager.Instance.InfoPaneSet += ActionManager_InfoPaneSet;
         ActionManager.Instance.CurrStudent.BookmarksChanged += Student_BookmarksChanged;
         ActionManager.Instance.OnViewChange += ActionManager_OnViewChange;
         ActionManager.Instance.OnShowHelp += Instance_OnShowHelp;
      }

      void Instance_OnShowHelp(object sender, EventArgs e)
      {
         showHelp();
      }

      private void ActionManager_OnViewChange(object sender, ViewChangeArgs e)
      {
         tabs.SelectedItem = (TabItem)tabs.FindName(e.view.ToString());
      }

      void Student_BookmarksChanged(object sender, EventArgs e)
      {
         showBookmarks();
      }
      void Instance_MissingPrereqs(object sender, MissingPrereqArgs e)
      {
         List<IFilter<Course>> preReqs = ActionManager.Instance.CurrStudent.Schedule.GetMissingPreReqs(e.Course);
         string str = "";
         foreach (IFilter<Course> filter in preReqs) {
            str += Filter<Course>.PrettyString(filter) + "\n";
         }
         infoBox.SetText("Missing Prerequisites for " + e.Course.Course + "\n" + str);
      }

      void ActionManager_InfoPaneSet(object sender, InfoPaneSetArgs e)
      {
         infoBox.SetText(e.Text);
      }

      protected override void OnClosed(EventArgs e)
      {
         closeDebugWindow();
         closePopUp();
         closeStartScreen();
         base.OnClosed(e);
      }


      public void showBookmarks()
      {
         String marks = "";
         List<Course> bookmarked = ActionManager.Instance.CurrStudent.bookmarks;


         for (int i = 0; i < bookmarked.Count; i++)
         {
            Course course = bookmarked[i];
            marks += "" + course.Dept.Name + "(" + course.DeptAbv + ")" + " " + course.Number + " - " + course.Name + "\n";
         }

         bookmarks.SetText(marks);
      }

      public void showInfo(Course course)
      {
         infoBox.SetText("" + course.Dept.Name + "(" + course.DeptAbv + ")" + " " + course.Number + "\n" +
             course.Description);
      }


      private void loadFile()
      {
         String filename = getFile();
         ScheduleLoader.LoadSchedule(ActionManager.Instance.CurrStudent, filename);
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

      private void closePopUp()
      {
         if (popUp != null)
         {
            popUp.Close();
            popUp = null;
         }
      }


      private void closeStartScreen()
      {
         if (starter != null)
         {
            starter.Close();
            starter = null;
         }

      }


      private void showHelp()
      {
         if (popUp == null)
         {
            popUp = new HelpWindow();
            popUp.Closed += (sender, e) => popUp = null;
         }

         popUp.Show();
      }

      private void showStartScreen() {
         if (starter == null) {
            starter = new StartScreen();
            starter.Show();         
         }
         
      }


      private void Button_Click_1(object sender, RoutedEventArgs e)
      {
         showHelp();
      }

      private void Button_Click_2(object sender, RoutedEventArgs e)
      {
         loadFile();
      }
   }
}