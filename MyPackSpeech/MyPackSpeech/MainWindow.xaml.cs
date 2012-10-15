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

       private DegreeCatalog degrees;
       SpeechSynthesizer reader;
       private SpeechRecognitionEngine recognitionEngine;
       private CommandGrammar grammar;


      public MainWindow()
      {
          InitializeComponent();
          setupCourses();
      }

      private void setupCourses()
      {
          degrees = new DegreeCatalog();
          reader = new SpeechSynthesizer();
          recognitionEngine = new SpeechRecognitionEngine();
          grammar = new CommandGrammar(CourseCatalog.Instance.Courses);
          txtOutput.Text += "Number of Classes: " + CourseCatalog.Instance.Courses.Count;

          recognitionEngine.LoadGrammar(grammar.grammar);
          recognitionEngine.SetInputToDefaultAudioDevice();
          recognitionEngine.SpeechRecognized += recognitionEngine_SpeechRecognized;
      }
      void recognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs args)
      {
          reader.SpeakAsync(args.Result.Text);

          txtOutput.Text += "Command Found:" + args.Result.Text + "\n";
          if (args.Result.Semantics.ContainsKey("add"))
          {
              reader.SpeakAsync("Adding that class.");

          }
          if (args.Result.Semantics.ContainsKey("remove"))
          {
              reader.SpeakAsync("Removing that class.");

          }

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
      }

      int currReq = -1;
      private void btnNextClick(object sender, EventArgs e)
      {
         btnNext.HorizontalContentAlignment = HorizontalAlignment.Stretch;
         currReq++;
         currReq %= degrees.degrees[0].Requirements.Count;
         DegreeRequirement req = degrees.degrees[0].Requirements[currReq];
         btnNext.Content = req.Category.Name;
         CourseCatalog.Instance.Filter = req.CourseRequirement;

      }

      private void button1_Click(object sender, RoutedEventArgs e)
      {
          try
          {
              recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
              txtOutput.Text = "Started\n";
          } catch (System.InvalidOperationException) {
              Console.WriteLine("Speech has already been started");          
          }
      }

      private void txtOutput_TextChanged(object sender, TextChangedEventArgs e)
      {

      }
   }
}
