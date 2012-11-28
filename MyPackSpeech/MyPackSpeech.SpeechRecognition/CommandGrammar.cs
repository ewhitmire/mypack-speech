using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using MyPackSpeech.DataManager.Data;
using MyPackSpeech.DataManager;



namespace MyPackSpeech.SpeechRecognition
{
   public class CommandGrammar
   {
      public Grammar grammar;
      private List<Course> classList;
      private List<Department> depts;
      private GrammarBuilder semester;
      private Choices course;
      private GrammarBuilder reqs;
      private GrammarBuilder pleasantries;


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
         this.pleasantries = buildIntroGrammar();
         this.semester = buildSemesterGrammar();
         this.course = buildCourseGrammar();
         this.reqs = buildRequirementGrammars();
      }

      private GrammarBuilder buildIntroGrammar()
      {

         Choices starts = new Choices("I'd like a", "I'd like to", "I would like to",
             "I want to", "Would you", "Would you please", "please", "How about", "Let's");

         GrammarBuilder intro = new GrammarBuilder();
         intro.Append("now", 0, 1);
         intro.Append(starts, 0, 1);
         return intro;
      }

      private Choices buildCourseGrammar()
      {

         SemanticResultKey anaphora = new SemanticResultKey(Slots.CourseAnaphora.ToString(), new SemanticResultValue("it", "it"));


         //Class: a class, this class, that class, that other class
         Choices deptChoices = new Choices();
         SemanticResultValue deptsRV;
         for (int i = 0; i < this.depts.Count; i++)
         {
            deptsRV = new SemanticResultValue(this.depts[i].Abv, this.depts[i].Abv);
            
            //deptsRV = new SemanticResultValue(String.Join(".", this.depts[i].Abv.ToCharArray()), this.depts[i].Abv);
            deptChoices.Add(deptsRV);

            deptsRV = new SemanticResultValue(this.depts[i].Name, this.depts[i].Abv);
            deptChoices.Add(deptsRV);
         }

         SemanticResultKey deptSemKey = new SemanticResultKey(Slots.Department.ToString(), deptChoices);

         //Class: a class, this class, that class, that other class
         Choices classNumbers = new Choices();
         foreach (var numbersRV in classList.Select(c => c.Number).Distinct().Select(n => new SemanticResultValue(n.ToString(), n)))
         {
            classNumbers.Add(numbersRV);
         }
         SemanticResultKey numbersSemKey = new SemanticResultKey(Slots.Number.ToString(), classNumbers);

         GrammarBuilder course = new GrammarBuilder();
         course.Append(deptSemKey);
         course.Append(numbersSemKey);



         Choices courseNameChoices = new Choices();
         foreach (Course c in CourseCatalog.Instance.Courses)
         {
            courseNameChoices.Add(new SemanticResultValue(c.Name, c.ToString()));
         }
         SemanticResultKey courseNameKey = new SemanticResultKey(Slots.CourseName.ToString(), courseNameChoices);

         GrammarBuilder courseGrammar = new GrammarBuilder();
         Choices courseOrAnaphora = new Choices();
         courseOrAnaphora.Add(course);
         courseOrAnaphora.Add(anaphora);
         courseOrAnaphora.Add(courseNameKey);

         return courseOrAnaphora;
      }

      private GrammarBuilder buildRequirementGrammars()
      {
         Choices reqs = new Choices();
         //TODO restrict to current degree
         foreach (DegreeProgram degreeProgram in DegreeCatalog.Instance.Degrees)
         {
            foreach (DegreeRequirementCategory cat in degreeProgram.GetCategories())
            {
               reqs.Add(new SemanticResultValue(cat.Name, cat.Name));
               foreach (DegreeRequirement req in degreeProgram.GetRequirementsForCategory(cat))
               {
                  if (req.Name != null)
                  {
                     reqs.Add(new SemanticResultValue(req.Name, req.Name));
                  }
               }
            }
         }

         SemanticResultKey reqKey = new SemanticResultKey(Slots.Requirement.ToString(), reqs);

         Choices classWord = new Choices();
         classWord.Add("class");
         classWord.Add("classes");
         classWord.Add("course");
         classWord.Add("courses");
         classWord.Add("requirement");
         classWord.Add("requirements");

         GrammarBuilder builder = new GrammarBuilder();
         builder.Append(reqKey);
         builder.Append(classWord);

         return builder;

      }


      private GrammarBuilder buildCourseGrammarNested()
      {
         //Class: a class, this class, that class, that other class
         Choices deptChoices = new Choices();
         SemanticResultValue deptsRV;
         for (int i = 0; i < this.depts.Count; i++)
         {
            Department dept = depts[i];
            Choices classNumbers = new Choices();
            foreach (var numbersRV in classList.Where(c => c.DeptAbv == dept.Abv).Select(c => c.Number).Distinct().Select(n => new SemanticResultValue(n.ToString(), n)))
            {
               classNumbers.Add(numbersRV);
            }

            deptsRV = new SemanticResultValue(dept.Abv, dept.Abv);
            GrammarBuilder deptsGmr = deptsRV.ToGrammarBuilder();
            deptsGmr.Append(classNumbers);
            deptChoices.Add(deptsGmr);

            deptsRV = new SemanticResultValue(dept.Name, dept.Abv);
            deptsGmr = deptsRV.ToGrammarBuilder();
            deptsGmr.Append(classNumbers);
            deptChoices.Add(deptsGmr);
         }

         SemanticResultKey deptSemKey = new SemanticResultKey(Slots.Department.ToString(), deptChoices);
         GrammarBuilder course = new GrammarBuilder();
         course.Append(deptChoices.ToGrammarBuilder());

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

         SemanticResultKey semesterSemKey = new SemanticResultKey(Slots.Semester.ToString(), semestersChoices);

         GrammarBuilder semester = new GrammarBuilder();

         semester.Append(prepositions, 0, 1);
         semester.Append(semesterSemKey);
         semester.Append("schedule", 0, 1);

         // "2013" OR "of 2013"
         Choices yearPrep = new Choices("of", "to", "from", "in");

         // year
         Choices years = new Choices();
         SemanticResultValue yearRV;
         //int currentYear = 2012;
         for (int year = 2000; year < 2020; year++)
         {
            yearRV = new SemanticResultValue(year.ToString(), year.ToString());
            years.Add(yearRV);

         }
         SemanticResultKey yearSemKey = new SemanticResultKey(Slots.Year.ToString(), years);

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
         GrammarBuilder error = errorCommand();
         GrammarBuilder setSemester = semesterCommand();
         GrammarBuilder inquire = inquireCommand();
         GrammarBuilder bookmark = bookmarkCommand();
         GrammarBuilder help = helpCommand();
         GrammarBuilder view = viewCommand();

         //now build the complete pattern...
         Choices commandChoices = new Choices();
         commandChoices.Add(add);
         commandChoices.Add(remove);
         commandChoices.Add(move);
         commandChoices.Add(error);
         commandChoices.Add(setSemester);
         commandChoices.Add(inquire);
         commandChoices.Add(course);
         commandChoices.Add(bookmark);
         commandChoices.Add(help);
         commandChoices.Add(view);
         
         GrammarBuilder systemRequest = new GrammarBuilder();
         systemRequest.Append(commandChoices);

         Grammar testGrammar = new Grammar(systemRequest);
         this.grammar = testGrammar;
      }

      private GrammarBuilder semesterCommand()
      {
         //<pleasantries> <command> <Semester>? <Year>?
         Choices commands = new Choices();
         commands.Add(new SemanticResultValue("set semester", (int)CommandTypes.SetSemester));
         commands.Add(new SemanticResultValue("semester", (int)CommandTypes.SetSemester));
         commands.Add(new SemanticResultValue("go to", (int)CommandTypes.SetSemester));
         SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey, 0, 1);
         finalCommand.Append(this.semester);

         return finalCommand;
      }

      private GrammarBuilder helpCommand()
      {
         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         commandSRV = new SemanticResultValue("Help", (int)CommandTypes.Help);
         commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("Help me", (int)CommandTypes.Help);
         commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("What do I do", (int)CommandTypes.Help);
         commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("I don't know what to say", (int)CommandTypes.Help);
         commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("I do not know what to say", (int)CommandTypes.Help);
         commands.Add(commandSRV);


         SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(commandSemKey);

         return finalCommand;      
      
      }

      private GrammarBuilder errorCommand()
      {
         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         //commandSRV = new SemanticResultValue("no", (int)CommandTypes.Undo);
         //commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("undo", (int)CommandTypes.Undo);
         commands.Add(commandSRV);
         SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);

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
         SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);


         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.course, 0, 1);
         finalCommand.Append(this.semester, 0, 1);

         return finalCommand;
      }

      private GrammarBuilder bookmarkCommand()
      {
         //<pleasantries> <command> <CLASS> <prep> <Time><year>
         //Pleasantries: I'd like to, please, I want to, would you
         //Command: Add, Remove
         //Class: a class, this class, that class, that other class
         //When: to Spring 2012

         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         commandSRV = new SemanticResultValue("bookmark", (int)CommandTypes.Bookmark);
         commands.Add(commandSRV);
         SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);


         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.course, 0, 1);

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
         SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);

         //SemanticResultKey course1 = new SemanticResultKey(Slots.Course1.ToString(), this.course);

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.course, 0, 1);
         finalCommand.Append(this.semester, 0, 1);

         return finalCommand;
      }

      private GrammarBuilder moveCommand()
      {
         Choices commands = new Choices();
         SemanticResultValue commandSRV;
         commandSRV = new SemanticResultValue("move", (int)CommandTypes.Move);
         commands.Add(commandSRV);
         commandSRV = new SemanticResultValue("switch", (int)CommandTypes.Move);
         commands.Add(commandSRV);
         SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);

         Choices preps = new Choices("to", "in");
         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.course, 0, 1);
         finalCommand.Append(preps, 0, 1);
         finalCommand.Append(this.semester, 0, 1);

         return finalCommand;
      }

      private GrammarBuilder inquireCommand()
      {
         Choices commands = new Choices();
         commands.Add(new SemanticResultValue("what", (int)CommandTypes.Inquire));
         commands.Add(new SemanticResultValue("which", (int)CommandTypes.Inquire));
         commands.Add(new SemanticResultValue("how many", (int)CommandTypes.Inquire));
         commands.Add(new SemanticResultValue("are there any", (int)CommandTypes.Inquire));
         commands.Add(new SemanticResultValue("what are my", (int)CommandTypes.Inquire));
         commands.Add(new SemanticResultValue("what are the", (int)CommandTypes.Inquire));
         SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);


         Choices verb = new Choices();
         verb.Add("can");
         verb.Add("are");
         verb.Add("should");
         verb.Add("do");
         verb.Add("that");

         Choices suffix = new Choices();
         suffix.Add("I take");
         suffix.Add("there");
         suffix.Add("I need");
         suffix.Add("I need to take");
         suffix.Add("I still need");
         suffix.Add("I still need to take");

         Choices graduate = new Choices();
         graduate.Add("for graduation");
         graduate.Add("to graduate");

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(commandSemKey);
         finalCommand.Append(this.reqs, 0, 1);
         finalCommand.Append(verb, 0, 1);
         finalCommand.Append(suffix, 0, 1);
         finalCommand.Append(graduate, 0, 1);
         return finalCommand;
      }

      private GrammarBuilder viewCommand()
      {
         Choices commands = new Choices();
         commands.Add(new SemanticResultValue("view", (int)CommandTypes.View));
         SemanticResultKey commandSemKey = new SemanticResultKey(Slots.Command.ToString(), commands);

         Choices preCommand = new Choices();
         preCommand.Add("switch to");

         Choices views = new Choices();
         views.Add(new SemanticResultValue("semester", (int)Views.Semester));
         views.Add(new SemanticResultValue("requirements", (int)Views.Requirements));

         SemanticResultKey viewSemKey = new SemanticResultKey(Slots.ViewName.ToString(), views);

         // put the whole command together
         GrammarBuilder finalCommand = new GrammarBuilder();
         finalCommand.Append(this.pleasantries, 0, 1);
         finalCommand.Append(preCommand, 0, 1);
         finalCommand.Append(viewSemKey);
         finalCommand.Append(commandSemKey);
         return finalCommand;
      }
   }
}