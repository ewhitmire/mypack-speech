﻿using MyPackSpeech.DataManager;
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
         DegreeProgram degree = DialogManager.Instance.CurrStudent.Degree;
         if (!Semantics.ContainsKey(Slots.Requirement.ToString()))
         {
            // What do I need to graduate?
            
            IEnumerable<DegreeRequirement> reqs = degree.Requirements.Where(r => r.Fulfillment == null);
            String paneText = "";
            foreach (DegreeRequirement req in reqs)
            {
               paneText += req.ToGeneralRequirementsString() + "\n";
            }

            DialogManager.Instance.SetInfoPane(paneText);

            if (reqs.Count() == 0)
            {
               RecoManager.Instance.Say("You have fulfilled all of your requirements.");
            }
            else if (reqs.Count() > 5)
            {
               RecoManager.Instance.Say("You still need " + reqs.Count() + " requirements. Check the info pane for a list of possible classes.");
            }
            else
            {

               IEnumerable<String> reqNames = reqs.Select<DegreeRequirement, String>(r => r.ToSpeechString());
               RecoManager.Instance.Say("You still need " + reqs.Count() + " requirements, including " + SpeechUtils.MakeSpeechList(reqNames));
            }

            return true;
         }

         String reqName = Semantics.GetSlot(Slots.Requirement);
         DegreeRequirementCategory cat = degree.GetCategories().Find(c => c.Name.Equals(reqName));
         if (cat != null)
         {
            IEnumerable<DegreeRequirement> reqs = degree.GetRequirementsForCategory(cat);
            reqs = reqs.Where(r => r.Fulfillment == null);
            if (reqs.Count() == 0)
            {
               // all requirements fulfilled
               RecoManager.Instance.Say("You have already fulfilled all of your "+cat.Name+" requirements");
               DialogManager.Instance.SetInfoPane("");
            }
            else
            {

               String paneText = "";
               foreach (DegreeRequirement req in reqs)
               {
                  paneText += req.ToPrintedString() + "\n";
               }
               if (reqs.Count() > 5)
               {
                  RecoManager.Instance.Say("You still need " + reqs.Count() + " requirements. Check the info pane for a list of possible classes.");
               }
               else if (reqs.Count() == 1)
               {
                  IEnumerable<Course> courses = CourseCatalog.Instance.GetCourses(reqs.First().CourseRequirement);
                  IEnumerable<String> classNames = courses.Select<Course, String>(c => c.Name);
                  RecoManager.Instance.Say("You have " + courses.Count() + " options for the "+reqs.First().ToSpeechString()+" requirement, including " + SpeechUtils.MakeSpeechList(classNames));

               }
               else
               {
                  IEnumerable<String> reqNames = reqs.Select<DegreeRequirement, String>(r => r.ToSpeechString());
                  RecoManager.Instance.Say("You still need " + reqs.Count() + " requirements, including " + SpeechUtils.MakeSpeechList(reqNames));
               }

               DialogManager.Instance.SetInfoPane(paneText);
            }
         }
         else
         {
            DegreeRequirement req = degree.Requirements.Find(r => reqName.Equals(r.Name));
            IEnumerable<Course> courses = CourseCatalog.Instance.GetCourses(req.CourseRequirement);
            String paneText = "";
            foreach (Course c in courses)
            {
               paneText += c.DeptAbv + c.Number + " - " + c.Name + "\n";
            }
            DialogManager.Instance.SetInfoPane(paneText);

            if (courses.Count() > 5)
            {
               RecoManager.Instance.Say("You have " + courses.Count() + " options to fulfill the " + req.Name + " requirement. Check the info pane for a list of possible classes.");
            }
            else
            {
               IEnumerable<String> classNames = courses.Select<Course, String>(c => c.Name);

               RecoManager.Instance.Say("You have " + courses.Count() + " options to fulfill the " + req.Name + " requirement, including " + SpeechUtils.MakeSpeechList(classNames));
            }
         }

         return true;
      }

      public override void GiveConfirmation()
      {
         return;
      }

      public override void Undo()
      {
         
      }
   }
}
