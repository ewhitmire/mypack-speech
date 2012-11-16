
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class DegreeProgram
   {
      public List<DegreeRequirement> Requirements { get; private set; }
      public List<DegreeRequirementCategory> GetCategories()
      {
         return Requirements.Select(r => r.Category).Distinct().ToList();
      }

      public IEnumerable<DegreeRequirement> GetRequirementsForCategory(DegreeRequirementCategory cat)
      {
         return Requirements.Where<DegreeRequirement>(r => r.Category.Equals(cat));
      }

      public String Name { get; private set; }

      public DegreeProgram(String degreeName)
      {
         Name = degreeName;
         Requirements = new List<DegreeRequirement>();
      }
   }
}
