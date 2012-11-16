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
	/// Interaction logic for ChartWindow.xaml
	/// </summary>
	public partial class DebugWindow : Window
	{
      private static DebugWindow instance;
		public DebugWindow()
		{
         instance = this;
			InitializeComponent();
		}
      public static void Trace(String message)
      {
         if (instance != null)
         {
            instance.debugOutput.AppendText(message+"\n");
         }
      }

      private void debugSpeechBtn_Click(object sender, RoutedEventArgs e)
      {
         RecoManager.Instance.TestText(debugSpeech.Text);
         debugSpeech.Text = "";
      }
	}
}
