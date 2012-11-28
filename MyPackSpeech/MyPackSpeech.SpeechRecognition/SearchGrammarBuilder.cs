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
                  KeyWords.Add(word);
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
            SemanticResultValue commandSRV;
            commandSRV = new SemanticResultValue("search", (int)CommandTypes.Search);
            commands.Add(commandSRV);
            commandSRV = new SemanticResultValue("find", (int)CommandTypes.Search);
            SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);


            // put the whole command together
            Choices keywordChoices = new Choices();

            foreach (string keyword in KeyWords)
            {
               keywordChoices.Add(new SemanticResultValue(keyword, keyword));
            }            

            SemanticResultKey keywordSemKey = new SemanticResultKey(Slots.KeyWords.ToString(), keywordChoices);

            grammar.Append(commandSemKey, 1, 1);
            grammar.Append(keywordSemKey, 1, 5);
         }
      }

      internal static bool Process(SemanticValue semanticValue)
      {
         return false;
      }
   }
}
