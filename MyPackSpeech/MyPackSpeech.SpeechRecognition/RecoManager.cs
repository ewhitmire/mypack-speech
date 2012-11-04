using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using MyPackSpeech.DataManager;

namespace MyPackSpeech.SpeechRecognition
{

	public class RecoManager
	{

		private SpeechRecognitionEngine recognitionEngine;
		public SpeechSynthesizer reader;
		private CommandGrammar grammar;
      private int tries = 0;

		public delegate void SpeechRecognizedHandler(object sender, SpeechRecognizedEventArgs ca);
		public event SpeechRecognizedHandler SpeechRecognized;



		private static RecoManager instance = null;
		public static RecoManager Instance
		{
			get
			{
				if (instance == null)
					instance = new RecoManager();
				return instance;
			}
		}

		protected RecoManager()
		{
			reader = new SpeechSynthesizer();
			recognitionEngine = new SpeechRecognitionEngine();
			grammar = new CommandGrammar(CourseCatalog.Instance.Courses);
			recognitionEngine.LoadGrammar(grammar.grammar);
			recognitionEngine.SetInputToDefaultAudioDevice();
			recognitionEngine.SpeechRecognized += recognitionEngine_SpeechRecognized;
         recognitionEngine.SpeechRecognitionRejected += this.recognitionEngine_SpeechRejected;
		}

      private void recognitionEngine_SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e) {
         tries++;
         RecognitionResult result = e.Result;
         string rejected = "Rejected: " + (result == null ? string.Empty : result.Text + " " + result.Confidence);
         recognitionEngine.RecognizeAsyncStop();

         if (tries < 2)
         {
            reader.Speak("I'm sorry, I didn't understand you.");
         }
         else {
            reader.Speak("That may not be a valid command.  Try saying something like. I would like to Add CSC 5 91 to my fall semester 2012.");
         }
         
         recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
         System.Console.WriteLine(rejected);
      }

		public void Start()
		{
			recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
		}
		void recognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs args)
		{
         tries = 0;
			reader.SpeakAsyncCancelAll();
			//reader.SpeakAsync(args.Result.Text);
			ActionManager.Instance.ProcessResult(args.Result);
			if (SpeechRecognized != null)
			{
				SpeechRecognized(this, args);
			}
		}

		public Grammar AddCourseGrammar()
		{
			return new Grammar("");
		}
	}
}
