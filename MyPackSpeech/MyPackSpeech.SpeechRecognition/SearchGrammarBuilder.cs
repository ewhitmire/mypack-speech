using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;

namespace MyPackSpeech.SpeechRecognition
{
   public static class SearchGrammarBuilder
   {
      private static GrammarBuilder grammar;
      public static GrammarBuilder Grammar
      {
         get
         {
            if (grammar == null)
               buildGrammar();
            return grammar;
         }
      }
      public static HashSet<string> KeyWords { get; private set; }
      public const string KeyWordFile = "keywords.csv";

      static SearchGrammarBuilder()
      {
         KeyWords = new HashSet<string>();

         if (File.Exists(KeyWordFile))
         {
            using (StreamReader sr = new StreamReader(File.OpenRead(KeyWordFile)))
            {
               string word;

               while ((word = sr.ReadLine()) != null)
               {
                  if (word.Length > 2)
                  {
                     KeyWords.Add(word);
                  }
               }
            }
         }
      }

      private static void buildGrammar()
      {
         if (grammar == null)
         {
            grammar = new GrammarBuilder();
            Choices commands = new Choices();
            commands.Add(new SemanticResultValue("search", (int)CommandTypes.Search));
            commands.Add(new SemanticResultValue("search for", (int)CommandTypes.Search));
            commands.Add(new SemanticResultValue("find", (int)CommandTypes.Search));
            commands.Add(new SemanticResultValue("I want to take", (int)CommandTypes.Search));
            commands.Add(new SemanticResultValue("I want to take a", (int)CommandTypes.Search));
            commands.Add(new SemanticResultValue("What", (int)CommandTypes.Search));
            SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);


            // put the whole command together
            Choices keywordChoices = new Choices();

            foreach (string keyword in KeyWords)
            {
               keywordChoices.Add(new SemanticResultValue(keyword, keyword));
            }            

            SemanticResultKey keywordSemKey = new SemanticResultKey(Slots.KeyWords.ToString(), keywordChoices);

            Choices suffix = new Choices();
            suffix.Add("classes");
            suffix.Add("class");
            suffix.Add("course");
            suffix.Add("courses");

            Choices suffix2 = new Choices();
            suffix2.Add("can I take");
            suffix2.Add("are there");

            grammar.Append(commandSemKey);
            grammar.Append(keywordSemKey);
            grammar.Append(suffix, 0, 1);
            grammar.Append(suffix2, 0, 1);
         }
      }

      internal static bool Process(SemanticValue semanticValue)
      {
         return false;
      }
   }
}
