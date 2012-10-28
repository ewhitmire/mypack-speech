using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using MyPackSpeech.DataManager.Data;



namespace MyPackSpeech.SpeechRecognition
{
   public class CommandGrammar
   {
      public Grammar grammar;
      private List<Course> classList;
      private List<Department> depts;
      private GrammarBuilder semester;
      private GrammarBuilder course;
      private GrammarBuilder course2;
      private Choices pleasantries;


      public CommandGrammar()
      {
         buildCommonGrammars();
         buildCommandGrammar();
      }

      public CommandGrammar(List<Course> courses)
      {
         this.classList = courses;
         this.depts = new List<Department>();

         parseClasses();
         buildCommonGrammars();
         buildCommandGrammar();
      }
      private void buildCommonGrammars()
      {
         this.pleasantries = new Choices("I'd like a", "I'd like to", "I would like to",
             "I want to", "Would you", "Would you please", "please");

         this.semester = buildSemesterGrammar();
         this.course = buildCourseGrammar("");
         this.course2 = buildCourseGrammar("other_");

      }
      private GrammarBuilder buildCourseGrammar(String semKeyPrefix)
      {
         //Class: a class, this class, that class, that other class
         Choices deptChoices = new Choices();
         SemanticResultValue deptsRV;
         for (int i = 0; i < this.depts.Count; i++)
         {
            deptsRV = new SemanticResultValue(this.depts[i].Abv, this.depts[i].Abv);
            deptChoices.Add(deptsRV);
            deptsRV = new SemanticResultValue(this.depts[i].Name, this.depts[i].Abv);
            deptChoices.Add(deptsRV);

         }
         SemanticResultKey deptSemKey = new SemanticResultKey(semKeyPrefix+"department", deptChoices);


         //Class: a class, this class, that class, that other class
         Choices classNumbers = new Choices();
         SemanticResultValue numbersRV;
         for (int i = 100; i < 1000; i++)
         {
            numbersRV = new SemanticResultValue("" + i, i);
            classNumbers.Add(numbersRV);

         }
         SemanticResultKey numbersSemKey = new SemanticResultKey(semKeyPrefix+"numbers", classNumbers);

         GrammarBuilder course = new GrammarBuilder();
         course.Append(deptSemKey);
         course.Append(numbersSemKey);

         return course;
      }
      private GrammarBuilder buildSemesterGrammar()
      {
         Choices prepositions = new Choices("to", "to my", "to the", "in", "in the");

         // semesters
         Choices semestersChoices = new Choices();
         SemanticResultValue semestersRV;
         semestersRV = new SemanticResultValue("spring", "spring");
         semestersChoices.Add(semestersRV);
         semestersRV = new SemanticResultValue("fall", "fall");
         semestersChoices.Add(semestersRV);

         SemanticResultKey semesterSemKey = new SemanticResultKey("semester", semestersChoices);
        
         GrammarBuilder semester = new GrammarBuilder();

         semester.Append(prepositions, 0, 1);
         semester.Append(semesterSemKey);
         semester.Append("schedule", 0, 1);

         // "2013" OR "of 2013"
         Choices yearPrep = new Choices("of", "to", "from", "in");

         // year
         Choices years = new Choices();
         SemanticResultValue yearRV;
         int currentYear = 2012;
         for (int year = currentYear; year < currentYear + 6; year++)
         {
            yearRV = new SemanticResultValue(year.ToString(), year.ToString());
            years.Add(yearRV);

         }
         SemanticResultKey yearSemKey = new SemanticResultKey("year", years);

         GrammarBuilder yearGrammar = new GrammarBuilder();
         yearGrammar.Append(yearPrep, 0, 1);
         yearGrammar.Append(yearSemKey);
         yearGrammar.Append("schedule", 0, 1);

         GrammarBuilder semesterYear = new GrammarBuilder();
         semesterYear.Append(semester, 0, 1);
         semesterYear.Append(yearGrammar, 0, 1);
         return semesterYear;
      }

      private void parseClasses()
      {
         for (int i = 0; i < this.classList.Count; i++)
         {
            Course thisCourse = this.classList[i];
            if (!depts.Contains(thisCourse.Dept))
            {
               depts.Add(thisCourse.Dept);
            }
         }
      }

      private void buildCommandGrammar()
      {


         GrammarBuilder add = addCommand();
         GrammarBuilder remove = removeCommand();
         GrammarBuilder move = moveCommand();
         GrammarBuilder swap = swapCommand();
         GrammarBuilder error = errorCommand();


         //now build the complete pattern...
         Choices commandChoices = new Choices();
         commandChoices.Add(add);
         commandChoices.Add(remove);
         commandChoices.Add(move);
         commandChoices.Add(swap);
         commandChoices.Add(error);

         GrammarBuilder systemRequest = new GrammarBuilder();
         systemRequest.Append(commandChoices);

         Grammar testGrammar = new Grammar(systemRequest);
         this.grammar = testGrammar;
      }
      private GrammarBuilder errorCommand()
      {

         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         commandSRV = new SemanticResultValue("no", (int)CommandTypes.Undo);
         commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("undo", (int)CommandTypes.Undo);
         commands.Add(commandSRV);
         SemanticResultKey commandSemKey = new SemanticResultKey("command", commands);

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(commandSemKey);

         return finalCommand;
      }
      private GrammarBuilder addCommand()
      {
         //<pleasantries> <command> <CLASS> <prep> <Time><year>
         //Pleasantries: I'd like to, please, I want to, would you
         //Command: Add, Remove
         //Class: a class, this class, that class, that other class
         //When: to Spring 2012

         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         commandSRV = new SemanticResultValue("add", (int)CommandTypes.Add);
         commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("take", (int)CommandTypes.Add);
         commands.Add(commandSRV);
         SemanticResultKey commandSemKey = new SemanticResultKey("command", commands);

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.course);
         finalCommand.Append(this.semester);

         return finalCommand;
      }

      private GrammarBuilder removeCommand()
      {

         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         commandSRV = new SemanticResultValue("remove", (int)CommandTypes.Remove);
         commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("get rid of", (int)CommandTypes.Remove);
         commands.Add(commandSRV);
         SemanticResultKey commandSemKey = new SemanticResultKey("command", commands);

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.course);
         finalCommand.Append(this.semester);

         return finalCommand;
      }

      private GrammarBuilder moveCommand()
      {

         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         commandSRV = new SemanticResultValue("move", (int)CommandTypes.Move);
         commands.Add(commandSRV);
         SemanticResultKey commandSemKey = new SemanticResultKey("command", commands);

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.course);
         finalCommand.Append(this.semester);

         return finalCommand;
      }

      private GrammarBuilder swapCommand()
      {

         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         commandSRV = new SemanticResultValue("swap", (int)CommandTypes.Swap);
         commands.Add(commandSRV);
         SemanticResultKey commandSemKey = new SemanticResultKey("command", commands);

         

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.course);
         finalCommand.Append("with");
         finalCommand.Append(this.course2);

         return finalCommand;
      }


      private GrammarBuilder showCommand()
      {

         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         commandSRV = new SemanticResultValue("Show me", (int)CommandTypes.Show);
         commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("I want to see", (int)CommandTypes.Show);
         commands.Add(commandSRV);
         SemanticResultKey commandSemKey = new SemanticResultKey("command", commands);




         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.course);
         finalCommand.Append("with");
         finalCommand.Append(this.course2);

         return finalCommand;
      }


   }
}
