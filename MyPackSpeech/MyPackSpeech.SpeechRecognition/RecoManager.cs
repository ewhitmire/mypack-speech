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
      private bool isSpeechRecoActive = false;
      private bool isSpeechRecoStarted = false;

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

         recognitionEngine.SpeechRecognitionRejected += recognitionEngine_SpeechRejected;
         recognitionEngine.SpeechRecognized += recognitionEngine_SpeechRecognized;
         reader.SpeakCompleted += reader_SpeakCompleted;
         recognitionEngine.RecognizeCompleted += recognitionEngine_RecognizeCompleted;
      }

      void recognitionEngine_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
      {
         if (!isSpeechRecoActive && e.Cancelled)
         {
            recognitionEngine.SetInputToNull();
         }
      }

      void reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
      {
         ResumeSpeechReco();
      }

      public void PauseSpeechReco()
      {
         if (isSpeechRecoStarted)
         {
            reader.SpeakAsyncCancelAll();
            recognitionEngine.RecognizeAsyncCancel();
            isSpeechRecoStarted = false;
            Console.WriteLine("Speech stopped");
         }
      }

      public void ResumeSpeechReco()
      {
         if (!isSpeechRecoStarted && isSpeechRecoActive)
         {
            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            isSpeechRecoStarted = true;
            Console.WriteLine("Speech started");
         }
      }
      public void StopSpeechReco()
      {
         isSpeechRecoActive = false;
         PauseSpeechReco();
      }

      public void StartSpeechReco()
      {
         if (!isSpeechRecoStarted)
         {
            recognitionEngine.SetInputToDefaultAudioDevice();
         }
         isSpeechRecoActive = true;
         ResumeSpeechReco();
      }

      private void recognitionEngine_SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
      {
         tries++;
         RecognitionResult result = e.Result;
         string rejected = "Rejected: " + (result == null ? string.Empty : result.Text + " " + result.Confidence);
         //recognitionEngine.RecognizeAsyncStop();

         //if (tries < 2)
         //{
         //   reader.Speak("I'm sorry, I didn't understand you.");
         //}
         //else
         //{
         //   reader.Speak("That may not be a valid command.  Try saying something like. I would like to Add CSC 5 91 to my fall semester 2012.");
         //}

         //recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
         System.Console.WriteLine(rejected);
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

      internal void Say(string speech)
      {
         
         PauseSpeechReco();
         reader.SpeakAsync(speech);
         Console.WriteLine(speech);
      }

      public void TestText(string speech)
      {
         if (speech.Equals(""))
         {
            return;
         }

         if (!isSpeechRecoActive)
         {
            recognitionEngine.EmulateRecognizeAsync(speech);
         }
      }
   }
}
