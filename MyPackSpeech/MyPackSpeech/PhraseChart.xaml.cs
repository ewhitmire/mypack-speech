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
using MyPackSpeech.SpeechRecognition;
using System.Speech.Recognition;
using System.Windows.Forms.DataVisualization.Charting;
using System.ComponentModel;

namespace MyPackSpeech
{
	/// <summary>
	/// Interaction logic for PhraseChart.xaml
	/// </summary>
	public partial class PhraseChart : UserControl
	{
		public PhraseChart()
		{
			InitializeComponent();
			createSampleChart();
			if (DesignerProperties.GetIsInDesignMode(this) == false)
				setupEvents();
		}

		private void createSampleChart()
		{
			barChart.ChartAreas.Clear();
			barChart.ChartAreas.Add("area1");
			ChartArea chartArea = barChart.ChartAreas[0];
			chartArea.AxisX.MajorGrid.Enabled = false;
			chartArea.AxisY.MajorGrid.Enabled = false;
			chartArea.AxisY.Minimum = 0;
			chartArea.AxisY.Maximum = 1;
			barChart.Series.Clear();
			barChart.Series.Add("matches");
			Series series = barChart.Series[0];
			series.ChartType = SeriesChartType.Column;
			series.ChartArea = barChart.ChartAreas[0].Name;
			int x = 1;
			DataPoint pt = new DataPoint(series);
			pt.XValue = x;
			x++;
			pt.YValues = new double[] { .75 };
			pt.AxisLabel = "phrase 1";
			series.Points.Add(pt);
			pt.BorderColor = System.Drawing.Color.Black;
			int r = 255 - (int)(pt.YValues[0] * 255.0);
			pt.Color = System.Drawing.Color.FromArgb(r, r, r);

			pt = new DataPoint(series);
			pt.XValue = x;
			x++;
			pt.YValues = new double[] { .5 };
			pt.AxisLabel = "phrase 2";
			series.Points.Add(pt);
			pt.BorderColor = System.Drawing.Color.Black;
			r = 255 - (int)(pt.YValues[0] * 255.0);
			pt.Color = System.Drawing.Color.FromArgb(r, r, r);
		}

		private void setupEvents()
		{
			RecoManager.Instance.SpeechRecognized += Instance_SpeechRecognized;
		}

		void Instance_SpeechRecognized(object sender, SpeechRecognizedEventArgs ca)
		{
         DebugWindow.Trace("Command Found:" + ca.Result.Text + " (" + ca.Result.Confidence + ")");

			barChart.SuspendLayout();
			barChart.ChartAreas.Clear();
			barChart.ChartAreas.Add("area1");
			ChartArea chartArea = barChart.ChartAreas[0];
			chartArea.AxisX.MajorGrid.Enabled = false;
			chartArea.AxisY.MajorGrid.Enabled = false;
			barChart.Series.Clear();
			barChart.Series.Add("matches");
			Series series = barChart.Series[0];
			series.ChartArea = barChart.ChartAreas[0].Name;

			int x = 1;
         //DataPoint pt = new DataPoint(series);
         //pt.XValue = x;
         //x++;
         //pt.YValues = new double[] { ca.Result.Confidence };
         //pt.AxisLabel = ca.Result.Text;
         //pt.BorderColor = System.Drawing.Color.Black;
         //int r = 255 - (int)(pt.YValues[0] * 255.0);
         //pt.Color = System.Drawing.Color.FromArgb(r, r, r);

         //series.Points.Add(pt);
         //pt = null;
			foreach (RecognizedPhrase phrase in ca.Result.Alternates)
			{

            DebugWindow.Trace("Alternative: " + phrase.Text + " (" + phrase.Confidence + ")");
				DataPoint pAlt = new DataPoint(series);
				pAlt.XValue = x;
				pAlt.YValues = new double[] { phrase.Confidence };
				pAlt.AxisLabel = phrase.Text;
				x++;
				pAlt.BorderColor = System.Drawing.Color.Black;
				int altr = 255 - (int)(pAlt.YValues[0] * 255.0);
				pAlt.Color = System.Drawing.Color.FromArgb(altr, altr, altr);
            pAlt.Label = phrase.Confidence.ToString("G4");
            series.Points.Add(pAlt);

			}
			barChart.ResumeLayout();
		}
	}
}
