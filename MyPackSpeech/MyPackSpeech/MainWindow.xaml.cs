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
using Microsoft.Win32;
using MyPackSpeech.SpeechRecognition;
using MyPackSpeech.DataManager;
using MyPackSpeech.DataManager.Data;
using System.Collections.ObjectModel;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
       List<Course> bookmarked = new List<Course>();

      public MainWindow()
      {
         InitializeComponent();
         setupCourses();
         makeHeaders();
         makeGrids();
         makeRequirementTree();
         Course course = CourseCatalog.Instance.Courses[5];
         Course course2 = CourseCatalog.Instance.Courses[4250];
         Course course3 = CourseCatalog.Instance.Courses[1240];

         DegreeProgram dp = DegreeCatalog.Instance.Degrees[0];
         Student student = new Student(dp);
         ScheduledCourse myCourse1 = new ScheduledCourse(course, Semester.Spring, 2013, null);
         ScheduledCourse myCourse2 = new ScheduledCourse(course2, Semester.Spring, 2016, null);
         ScheduledCourse myCourse3 = new ScheduledCourse(course3, Semester.Fall, 2012, null);
         student.AddCourse(myCourse1);
         student.AddCourse(myCourse2);
         student.AddCourse(myCourse3);

         //RefreshSchedule(student.Schedule);

         //addClass(course, 3);
         //addClass(course2, 3);
         //addClass(course3, 7);
         ////removeClass(course);
         //moveClass(course2,7);
         //swapClasses(course3, course);
         //showInfo(course2);
         //addBookmark(course);
         //addBookmark(course2);
         //addBookmark(course3);
         //removeBookmark(course2);

      }
      private void ClearSchedule()
      {
         for (int i = 0; i < myGrid.Children.Count; i++)
         {
            TextBlock e = (TextBlock)myGrid.Children[i];
            e.Text = "";
         }
         for (int i = 0; i < myGrid2.Children.Count; i++)
         {
            TextBlock e = (TextBlock)myGrid2.Children[i];
            e.Text = "";
         }
      }
      public void RefreshSchedule(Schedule schedule) {
          //First semester is fall 2012
         ClearSchedule();
          for (int i = 0; i < schedule.Courses.Count; i++) {
              int column;
              int sem = 0;
              ScheduledCourse course = schedule.Courses[i];
              if(course.Semester == Semester.Fall) { 
                    sem = 1;
              }

             //2012: -1-0
             //2013: 1-2
             //2014: 3-4
             //2014: 5-6
             //2016: 7

              int year = course.Year - 2012;
              
             // 2 semesters for each year accept 2012
                 year = year * 2 - 1;

              column = year + sem;

              if (-1 < column && column < 8)
              {
                 addClass(course.Course, column);
              }
          }

      
      }

      public void makeRequirementTree()
      {
      
      }


      public void addBookmark(Course course) {
          bookmarked.Add(course);
          showBookmarks();
      }

      public void removeBookmark(Course course) {
          
          bookmarked.Remove(course);
          showBookmarks();

      }

      public void showBookmarks() {
          String marks = "";

          for (int i = 0; i < bookmarked.Count; i++) { 
            Course course = bookmarked[i];
            marks += "" + course.Dept.Name + "(" + course.DeptAbv + ")" + " " + course.Number + "\n";
          }

          bookmarks.Text = marks;
      }

      public void showInfo(Course course)
      {
          infoBox.Text = "" + course.Dept.Name + "(" + course.DeptAbv + ")" + " " + course.Number + "\n" + 
              course.Description;
      
      
      }

      public void makeGrids() {
          myGrid.ShowGridLines = true;

          for (int i = 0; i < 4; i++) {
              ColumnDefinition colDef1 = new ColumnDefinition();
              myGrid.ColumnDefinitions.Add(colDef1);
          }

          for (int i = 0; i < 5; i++) {
              RowDefinition rowDef1 = new RowDefinition();
              myGrid.RowDefinitions.Add(rowDef1);
          }

          for (int i = 0; i < myGrid.RowDefinitions.Count; i++) 
          {
              for (int j = 0; j < myGrid.ColumnDefinitions.Count; j++)
              {
                  TextBlock txt1 = new TextBlock();
                  txt1.Text = "";
                  txt1.FontSize = 12;
                  txt1.FontWeight = FontWeights.Bold;
                  Grid.SetColumn(txt1, j);
                  Grid.SetRow(txt1, i);

                  myGrid.Children.Add(txt1);
              }
          }





          myGrid2.ShowGridLines = true;
          // Define the Columns

          for (int i = 0; i < 4; i++)
          {
              ColumnDefinition colDef1 = new ColumnDefinition();
              myGrid2.ColumnDefinitions.Add(colDef1);
          }

          for (int i = 0; i < 5; i++)
          {
              RowDefinition rowDef1 = new RowDefinition();
              myGrid2.RowDefinitions.Add(rowDef1);
          }

          
          for (int i = 0; i < myGrid2.RowDefinitions.Count; i++)
          {
              for (int j = 0; j < myGrid2.ColumnDefinitions.Count; j++)
              {
                  TextBlock txt1 = new TextBlock();
                  txt1.Text = "";
                  txt1.FontSize = 12;
                  txt1.FontWeight = FontWeights.Bold;
                  Grid.SetColumn(txt1, j);
                  Grid.SetRow(txt1, i);
                  myGrid2.Children.Add(txt1);
              }
          }

      }

      UIElement getGridElement(Grid g, int r, int c)
      {
          for (int i = 0; i < g.Children.Count; i++)
          {
              UIElement e = g.Children[i];
              if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c)
                  return e;
          }
          return null;
      }

       public Boolean isEmptyCell(Grid g, int r, int c){
           TextBlock e = (TextBlock)getGridElement(g, r, c);
            return (e.Text.Equals(""));
       }

       public void addTextToCell(Grid g, int r, int c, String text) {
           TextBlock e = (TextBlock)getGridElement(g, r, c);
           e.Text = text;
       }

       public void removeTextFromCell(Grid g, int r, int c, String text) {
           TextBlock e = (TextBlock)getGridElement(g, r, c);
           e.Text = "";
       }

       public int getSemester(Course course)
       {
           String text = course.DeptAbv + course.Number + "-" + course.Name;

           for (int i = 0; i < myGrid.Children.Count; i++)
           {
               UIElement e = myGrid.Children[i];
               if (((TextBlock)e).Text.Equals(text))
               {
                   return Grid.GetColumn(e);
               }
           }

           for (int i = 0; i < myGrid2.Children.Count; i++)
           {
               UIElement e = myGrid2.Children[i];
               if (((TextBlock)e).Text.Equals(text))
               {
                   return Grid.GetColumn(e) + 4;
               }
           }
           return -1;
       }

       public void swapClasses(Course course1, Course course2) {
           int curSem1 = getSemester(course1);
           int curSem2 = getSemester(course2);

           if (curSem1 > -1 && curSem2 > -1) {
               removeClass(course1);
               removeClass(course2);
               addClass(course1, curSem2);
               addClass(course2, curSem1);
           }

           //Throw error, course not found
       }

       public void moveClass(Course course, int semester)
       {
           removeClass(course);
           addClass(course, semester);
       }
       public void removeClass(Course course) {
           Boolean success = removeClass(myGrid, course);
           if(!success) success = removeClass(myGrid2, course);

           //If still not successful throw error, class not found.
       
       }

       public Boolean removeClass(Grid g, Course course) {
           String text = course.DeptAbv + course.Number + "-" + course.Name;
           
           for (int i = 0; i < g.Children.Count; i++) {
               TextBlock e = (TextBlock)g.Children[i];
               if (e.Text.Equals(text)) {
                   e.Text = "";
                   return true;
               }
           }
           return false;
       }

       public void addClass(Course course, int semester)
      {
          if (semester < 4)
          {
              String text = course.DeptAbv + course.Number + "-" + course.Name;
              //length greater than 27???

              int i = 0;
              Boolean added = false;
              while (i < myGrid.RowDefinitions.Count && !added)
              {
                  if (isEmptyCell(myGrid, i, semester))
                  {
                      addTextToCell(myGrid, i, semester, text);
                      added = true;
                  }
                  i++;
              }
              //If !added throw too many classes error

          }
          else {
              semester -= 4;
              String text = course.DeptAbv + course.Number + "-" + course.Name;

              int i = 0;
              Boolean added = false;
              while (i < myGrid2.RowDefinitions.Count && !added)
              {
                  if (isEmptyCell(myGrid2, i, semester))
                  {
                      addTextToCell(myGrid2, i, semester, text);
                      added = true;
                  }
                  i++;
              }

              //If !added throw too many classes error
          
          }
      }
       
       public void makeHeaders()
      {



          //List<String> myClasses = new List<String>{ "1","2","3","4","5" };
          DataGridTextColumn mySemester1 = new DataGridTextColumn();
          mySemester1.Width = POW1.Width / 4 - 2;
          mySemester1.Header = "Fall 2012";
          POW1.Columns.Add(mySemester1);


          DataGridTextColumn mySemester2 = new DataGridTextColumn();
          mySemester2.Width = POW1.Width / 4 -2;
          mySemester2.Header = "Spring 2013";
          POW1.Columns.Add(mySemester2);

          
          DataGridTextColumn mySemester3 = new DataGridTextColumn();
          mySemester3.Width = POW1.Width / 4;
          mySemester3.Header = "Fall 2013";
          POW1.Columns.Add(mySemester3);
          
          DataGridTextColumn mySemester4 = new DataGridTextColumn();
          mySemester4.Width = POW1.Width / 4;
          mySemester4.Header = "Spring 2014";
          POW1.Columns.Add(mySemester4);

          DataGridTextColumn mySemester5 = new DataGridTextColumn();
          mySemester5.Width = POW2.Width / 4;
          mySemester5.Header = "Fall 2014";
          POW2.Columns.Add(mySemester5);

          DataGridTextColumn mySemester6 = new DataGridTextColumn();
          mySemester6.Width = POW2.Width / 4;
          mySemester6.Header = "Spring 2015";
          POW2.Columns.Add(mySemester6);          

          DataGridTextColumn mySemester7 = new DataGridTextColumn();
          mySemester7.Width = POW2.Width / 4;
          mySemester7.Header = "Fall 2015";
          POW2.Columns.Add(mySemester7);

          DataGridTextColumn mySemester8 = new DataGridTextColumn();
          mySemester8.Width = POW2.Width / 4;
          mySemester8.Header = "Spring 2016";
          POW2.Columns.Add(mySemester8);

    

          

      }
      public void WriteToOutputWindow(String text)
      {
         txtOutput.Text += text;
      }

      private void setupCourses()
      {
         txtOutput.Text += "Number of Classes: " + CourseCatalog.Instance.Courses.Count;
         
      }
      
      private void Load_Click(object sender, RoutedEventArgs e)
      {
         loadFile();
      }

      private void loadFile()
      {

      }

      private void loadCourses_Click(object sender, RoutedEventArgs e)
      {
         string file = getFile();
      }

      private void loadStudent_Click(object sender, RoutedEventArgs e)
      {
         string file = getFile();
      }

      private string getFile()
      {
          Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
         if (dlg.ShowDialog(this).GetValueOrDefault(false))
            return dlg.FileName;
         return string.Empty;
      }

      private void showReqs()
      {
      }


      private void showReqs_Click(object sender, RoutedEventArgs e)
      {
         showReqs();
      }

      private void showCourses_Click(object sender, RoutedEventArgs e)
      {
      }

      int currReq = -1;
      private void btnNextClick(object sender, EventArgs e)
      {
         //btnNext.HorizontalContentAlignment = HorizontalAlignment.Stretch;
         currReq++;

         currReq %= DegreeCatalog.Instance.Degrees[0].Requirements.Count;
         DegreeRequirement req = DegreeCatalog.Instance.Degrees[0].Requirements[currReq];
         btnNext.Content = req.Category.Name;
         CourseCatalog.Instance.Filter = req.CourseRequirement;

      }

      private void RecoManager_SpeechRecognized(object sender, SpeechRecognizedEventArgs args)
      {
         WriteToOutputWindow("Command Found:" + args.Result.Text + " (" + args.Result.Confidence + ")\n");
         foreach (RecognizedPhrase phrase in args.Result.Alternates)
         {
            WriteToOutputWindow("Alternative: " + phrase.Text + " (" + phrase.Confidence + ")\n");
         }
      }

      private void ActionManager_ActionDetected(object sender, ActionDetectedEventArgs args)
      {
         WriteToOutputWindow("Action Found:" + args.CommandType+"\n");
         Student student = args.Student;
         RefreshSchedule(student.Schedule);
      }

      bool recoStarted = false;
      Student student;
      private void button1_Click(object sender, RoutedEventArgs e)
      {
		  if (!recoStarted)
		  {
			  try
			  {
              student = new Student(DegreeCatalog.Instance.Degrees[0]);
				  recoStarted = true;
				  RecoManager.Instance.Start();
				  RecoManager.Instance.SpeechRecognized += new RecoManager.SpeechRecognizedHandler(RecoManager_SpeechRecognized);
				  ActionManager.Instance.ActionDetected += ActionManager_ActionDetected;
				  txtOutput.Text = "Started\n";
				  recoStarted = true;
			  }
			  catch (System.InvalidOperationException)
			  {
				  Console.WriteLine("Speech has already been started");
			  }
		  }

		 ChartWindow cw = new ChartWindow();
		 cw.Show();
      }
   }
}