
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class DegreeProgram
   {
      public List<DegreeRequirement> requirements { get; private set; }
      public String name { get; private set; }

      public DegreeProgram(String degreeName)
      {
         name = degreeName;
         requirements = new List<DegreeRequirement>();
      }
   }
}
