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
          List<Course> bookmarked = new List<Course>();

         public MainWindow()
         {
         InitializeComponent();
         setupCourses();

         RecoManager.Instance.Start();
         DebugWindow.Trace("Started");

         DebugWindow cw = new DebugWindow();
         cw.Show();

      }



         public void addBookmark(Course course)
         {
             bookmarked.Add(course);
             showBookmarks();
         }

         public void removeBookmark(Course course) {
          
             bookmarked.Remove(course);
             showBookmarks();

         }

         public void showBookmarks() {
             String marks = "";

             for (int i = 0; i < bookmarked.Count; i++) { 
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
    
         
         private void setupCourses()
         {
            //txtOutput.Text += "Number of Classes: " + CourseCatalog.Instance.Courses.Count;
         
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
      private void showReqs()
      {
      }


         private void showReqs_Click(object sender, RoutedEventArgs e)
         {
            showReqs();
         }

         private void showCourses_Click(object sender, RoutedEventArgs e)
         {
         }

// <<<<<<< HEAD
         // int currReq = -1;
         // private void btnNextClick(object sender, EventArgs e)
         // {
            // //btnNext.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            // currReq++;

            // currReq %= DegreeCatalog.Instance.Degrees[0].Requirements.Count;
            // DegreeRequirement req = DegreeCatalog.Instance.Degrees[0].Requirements[currReq];
            // btnNext.Content = req.Category.Name;
            // CourseCatalog.Instance.Filter = req.CourseRequirement;

         // }

         // private void RecoManager_SpeechRecognized(object sender, SpeechRecognizedEventArgs args)
         // {
            // WriteToOutputWindow("Command Found:" + args.Result.Text + " (" + args.Result.Confidence + ")\n");
            // foreach (RecognizedPhrase phrase in args.Result.Alternates)
            // {
               // WriteToOutputWindow("Alternative: " + phrase.Text + " (" + phrase.Confidence + ")\n");
            // }
         // }

         // private void ActionManager_ActionDetected(object sender, ActionDetectedEventArgs args)
         // {
            // WriteToOutputWindow("Action Found:" + args.CommandType+"\n");
            // Student student = args.Student;
            // RefreshSchedule(student.Schedule);
         // }

         // bool recoStarted = false;
         // Student student;
         // private void button1_Click(object sender, RoutedEventArgs e)
         // {
		     // if (!recoStarted)
		     // {
			     // try
			     // {
                 // student = new Student(DegreeCatalog.Instance.Degrees[0]);
				     // recoStarted = true;
				     // RecoManager.Instance.Start();
				     // RecoManager.Instance.SpeechRecognized += new RecoManager.SpeechRecognizedHandler(RecoManager_SpeechRecognized);
				     // ActionManager.Instance.ActionDetected += ActionManager_ActionDetected;
				     // txtOutput.Text = "Started\n";
				     // recoStarted = true;
			     // }
			     // catch (System.InvalidOperationException)
			     // {
				     // Console.WriteLine("Speech has already been started");
			     // }
		     // }

		    // ChartWindow cw = new ChartWindow();
		    // cw.Show();
         // }
      // }
   // }
// =======

   }
}
