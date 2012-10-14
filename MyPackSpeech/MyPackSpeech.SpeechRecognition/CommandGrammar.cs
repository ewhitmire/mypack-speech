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

        public CommandGrammar() 
        {
            buildCommandGrammar();
        }

        public CommandGrammar(List<Course> courses)
        {
            this.classList = courses;
            this.depts = new List<Department>();

            parseClasses();
            buildCommandGrammar();
        }


        private void parseClasses() 
        {
            for (int i = 0; i < this.classList.Count; i++ ) 
            {
                Course thisCourse = this.classList[i];
                if(!depts.Contains(thisCourse.Dept)){
                    depts.Add(thisCourse.Dept);
                }

            }
        
        
        }
        private void buildCommandGrammar()
        {


            GrammarBuilder add = addCommand();


            //Might be needed for further commands that have a different form
            //In which case it will be systemRequest.Append(permutationList);

            GrammarBuilder permutationList = new GrammarBuilder();
            permutationList.Append(add);

            //now build the complete pattern...
            GrammarBuilder systemRequest = new GrammarBuilder();

            systemRequest.Append(permutationList, 0, 1);

            Grammar testGrammar = new Grammar(systemRequest);
            this.grammar = testGrammar;
        }

        private GrammarBuilder addCommand()
        {
            //<pleasantries> <command> <CLASS> <prep> <Time><year>
            //Pleasentries: I'd like to, please, I want to, would you
            //Command: Add, Remove
            //Class: a class, this class, that class, that other class
            //Preposition: to,from, of
            //Time: to my schedule, to the spring semester, to my fall semester
            //Year: 2012, 2013, 2014, 2015

            //build the core set of choices
            Choices pleasentries = new Choices("I'd like a", "I'd like to", "I would like to",
                "I want to", "Would you", "Would you please", "please");
            SemanticResultKey pleasentriesSemKey = new SemanticResultKey("pleasentries", pleasentries);

            Choices commands = new Choices();
            SemanticResultValue commandSRV;
            commandSRV = new SemanticResultValue("add", "add");
            commands.Add(commandSRV);
            commandSRV = new SemanticResultValue("remove", "remove");
            commands.Add(commandSRV);
            commandSRV = new SemanticResultValue("get rid of", "remove");
            commands.Add(commandSRV);
            SemanticResultKey commandSemKey = new SemanticResultKey("command", commands);


            //Class: a class, this class, that class, that other class
            Choices deptChoices = new Choices();
            SemanticResultValue deptsRV;
            for (int i = 0; i < this.depts.Count; i++ )
            {
                deptsRV = new SemanticResultValue(this.depts[i].Abv, this.depts[i].Abv);
                deptChoices.Add(deptsRV);
                deptsRV = new SemanticResultValue(this.depts[i].Name, this.depts[i].Abv);
                deptChoices.Add(deptsRV);

            }
            SemanticResultKey classesSemKey = new SemanticResultKey("department", deptChoices);

            Choices preposition = new Choices("to", "from", "of");
            SemanticResultKey prepSemKey = new SemanticResultKey("prepositions", preposition);



            // semesters
            Choices semesters = new Choices();
            SemanticResultValue semestersRV;
            semestersRV = new SemanticResultValue("my spring semester", "spring");
            semesters.Add(semestersRV);
            semestersRV = new SemanticResultValue("spring semester", "spring");
            semesters.Add(semestersRV);
            semestersRV = new SemanticResultValue("my fall semester", "fall");
            semesters.Add(semestersRV);
            semestersRV = new SemanticResultValue("fall semester", "fall");
            semesters.Add(semestersRV);

            SemanticResultKey semesterSemKey = new SemanticResultKey("semester", semesters);


            // year
            Choices years = new Choices();
            SemanticResultValue yearRV;
            yearRV = new SemanticResultValue("2012", "2012");
            years.Add(yearRV);
            yearRV = new SemanticResultValue("2013", "2013");
            years.Add(yearRV);
            yearRV = new SemanticResultValue("2014", "2014");
            years.Add(yearRV);
            yearRV = new SemanticResultValue("2015", "2015");
            years.Add(yearRV);
            yearRV = new SemanticResultValue("2016", "2016");
            years.Add(yearRV);
            SemanticResultKey yearSemKey = new SemanticResultKey("year", years);

            // "2013" OR "of 2013"
            GrammarBuilder prepYear = new GrammarBuilder();
            prepYear.Append(prepSemKey, 0, 1);
            prepYear.Append(yearSemKey);

            SemanticResultKey prepYearSemKey = new SemanticResultKey("year", years);

            // put the whole command together
            GrammarBuilder finalCommand = new GrammarBuilder();
            finalCommand.Append(pleasentries, 0, 1);
            finalCommand.Append(commandSemKey);
            finalCommand.Append(classesSemKey);
            finalCommand.Append(prepSemKey);
            finalCommand.Append(semesterSemKey);
            finalCommand.Append(prepYearSemKey, 0, 1);

            return finalCommand;
        }


    }
}
