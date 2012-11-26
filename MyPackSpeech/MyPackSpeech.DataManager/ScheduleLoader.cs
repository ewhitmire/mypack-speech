using MyPackSpeech.DataManager.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager
{
   public class ScheduleLoader
   {
      public static void LoadSchedule(Student student, String filepath)
      {
         using (StreamReader readFile = new StreamReader(filepath))
         {
            string line;
            string[] data;
            while ((line = readFile.ReadLine()) != null)
            {
               data = line.Split(',');
               if (data.Length >= 4)
               {
                  Department dept = CourseCatalog.Instance.GetDepartment(data[0]);
                  bool success = dept != null;
                  int number;
                  success &= int.TryParse(data[1], out number);
                  Semester sem;
                  success &= Enum.TryParse(data[2], out sem);
                  int year;
                  success &= int.TryParse(data[3], out year);
                  if (success)
                  {
                     Course course = CourseCatalog.Instance.GetCourse(dept, number);
                     ScheduledCourse scheduled_course = new ScheduledCourse(course, sem, year);
                     student.AddCourse(scheduled_course);
                  }
               }
            }
         }
      }
   }
}
