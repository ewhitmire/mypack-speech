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

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {

      public MainWindow()
      {
         InitializeComponent();
         setupCourses();
      }

      public void WriteToOutputWindow(String text)
      {
         txtOutput.Text += text;
      }

      private void setupCourses()
      {
         txtOutput.Text += "Number of Classes: " + CourseCatalog.Instance.Courses.Count;
         
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

      int currReq = -1;
      private void btnNextClick(object sender, EventArgs e)
      {
         btnNext.HorizontalContentAlignment = HorizontalAlignment.Stretch;
         currReq++;

         currReq %= DegreeCatalog.Instance.degrees[0].Requirements.Count;
         DegreeRequirement req = DegreeCatalog.Instance.degrees[0].Requirements[currReq];
         btnNext.Content = req.Category.Name;
         CourseCatalog.Instance.Filter = req.CourseRequirement;

      }

      private void RecoManager_SpeechRecognized(object sender, SpeechRecognizedEventArgs args)
      {
         WriteToOutputWindow("Command Found:" + args.Result.Text + " (" + args.Result.Confidence + ")\n");
         foreach (RecognizedPhrase phrase in args.Result.Alternates)
         {
            WriteToOutputWindow("Alternative: " + phrase.Text + " (" + phrase.Confidence + ")\n");
         }
      }

      private void ActionManager_ActionDetected(object sender, ActionDetectedEventArgs args)
      {
         WriteToOutputWindow("Action Found:" + args.type+"\n");
      }

      private void button1_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            RecoManager.Instance.Start();
            RecoManager.Instance.SpeechRecognized += new RecoManager.SpeechRecognizedHandler(RecoManager_SpeechRecognized);
            ActionManager.Instance.ActionDetected += new ActionManager.ActionDetectedHandler(ActionManager_ActionDetected);
            txtOutput.Text = "Started\n";
         }
         catch (System.InvalidOperationException)
         {
            Console.WriteLine("Speech has already been started");
         }
      }

      private void txtOutput_TextChanged(object sender, TextChangedEventArgs e)
      {

      }
   }
}
