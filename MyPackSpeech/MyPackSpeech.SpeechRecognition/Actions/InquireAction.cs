using MyPackSpeech.DataManager;
using MyPackSpeech.DataManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.SpeechRecognition.Actions
{
   class InquireAction : BaseAction
   {
      public override bool Perform()
      {
         if (!Semantics.ContainsKey(Slots.Requirement.ToString()))
         {
            return false;
         }

         String reqName = Semantics.GetSlot(Slots.Requirement);
         DegreeProgram degree = ActionManager.Instance.CurrStudent.Degree;
         DegreeRequirementCategory cat = degree.GetCategories().Find(c => c.Name.Equals(reqName));
         if (cat != null)
         {
            IEnumerable<DegreeRequirement> reqs = degree.GetRequirementsForCategory(cat);
            foreach (DegreeRequirement req in reqs)
            {
               Console.WriteLine(req.Name);

            }
         }
         else
         {
            DegreeRequirement req = degree.Requirements.Find(r => reqName.Equals(r.Name));
            IEnumerable<Course> courses = CourseCatalog.Instance.GetCourses(req.CourseRequirement);
            foreach (Course c in courses)
            {
               Console.WriteLine(c.Name);

            }
         }

         return true;
      }

      public override void Undo()
      {
         throw new NotImplementedException();
      }
   }
}
