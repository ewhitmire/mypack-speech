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

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for StartScreen.xaml
   /// </summary>
   public partial class StartScreen : Window
   {
      private static StartScreen instance;
      public StartScreen()
      {
         InitializeComponent();
         Loaded += StartScreen_Loaded;
         instance = this;
      }

      private void StartScreen_Loaded(object sender, RoutedEventArgs e)
      {
         IntroDialogue interaction = IntroDialogue.Instance;
         interaction.StartInteraction();
         interaction.OnComplete += interaction_OnComplete;
      }

      private void interaction_OnComplete(object sender, EventArgs e)
      {
         MainWindow win = new MainWindow();
         win.Show();
      }
      public static void CloseWindow()
      {
         instance.Close();
      }
   }
}
