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
		}

		public void Start()
		{
			recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
		}
		void recognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs args)
		{
			reader.SpeakAsyncCancelAll();
			reader.SpeakAsync(args.Result.Text);
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
