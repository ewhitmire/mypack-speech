using MyPackSpeech.SpeechRecognition;
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
using System.Windows.Shapes;
using System.Threading;
using System.Speech.Recognition;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for StartScreen.xaml
   /// </summary>
   public partial class StartScreen : Window
   {
      private static StartScreen instance;
      bool dataLoaded = false;
      public StartScreen()
      {
         InitializeComponent();
         Loaded += StartScreen_Loaded;
         instance = this;
      }

      private void StartScreen_Loaded(object sender, RoutedEventArgs e)
      {
         Thread thread = new Thread(new ThreadStart(loadData));
         thread.Start();
         IntroDialogue interaction = IntroDialogue.Instance;
         interaction.StartInteraction();
         interaction.OnComplete += interaction_OnComplete;
      }

      private void loadData()
      {
         ActionManager manager = ActionManager.Instance;
         GrammarBuilder search = SearchGrammarBuilder.Grammar;
         RecoManager reco = RecoManager.Instance;

         dataLoaded = true;
      }

      private void interaction_OnComplete(object sender, EventArgs e)
      {
         while (!dataLoaded)
         {
            Thread.Sleep(500);            
         }

         LoadMainWindow();
      }

      public static void CloseWindow()
      {
         instance.Close();
      }

      private void skip_Click(object sender, RoutedEventArgs e)
      {
         RecoManager.Instance.BeSilent();
         LoadMainWindow();
      }

      private void LoadMainWindow()
      {
         RecoManager.Instance.StopSpeechReco();
         MainWindow win = new MainWindow();
         win.Show();
      }
   }
}
