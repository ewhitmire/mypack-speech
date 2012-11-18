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
            String paneText = "";
            foreach (DegreeRequirement req in reqs)
            {
               paneText += req.Name + "\n";

            }
            ActionManager.Instance.SetInfoPane(paneText);
         }
         else
         {
            DegreeRequirement req = degree.Requirements.Find(r => reqName.Equals(r.Name));
            IEnumerable<Course> courses = CourseCatalog.Instance.GetCourses(req.CourseRequirement);
            String paneText = "";
            foreach (Course c in courses)
            {
               paneText += c.Name + "\n";
            }
            ActionManager.Instance.SetInfoPane(paneText);
         }

         return true;
      }

      public override void Undo()
      {
         
      }
   }
}
