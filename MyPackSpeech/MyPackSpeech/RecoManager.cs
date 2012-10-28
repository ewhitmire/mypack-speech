using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using MyPackSpeech.DataManager;

namespace MyPackSpeech.SpeechRecognition
{
   class RecoManager
   {

      private SpeechRecognitionEngine recognitionEngine;
      SpeechSynthesizer reader;
      private CommandGrammar grammar;


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

         MainWindow.Instance.WriteToOutputWindow("Command Found:" + args.Result.Text + " (" + args.Result.Confidence + ")\n");
         foreach (RecognizedPhrase phrase in args.Result.Alternates)
         {
            MainWindow.Instance.WriteToOutputWindow("Alternative: " + phrase.Text + " (" + phrase.Confidence + ")\n");
         }


      }

      public Grammar AddCourseGrammar()
      {
         return new Grammar("");
      }
   }
}
