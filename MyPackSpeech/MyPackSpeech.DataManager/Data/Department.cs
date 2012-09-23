using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPackSpeech.DataManager.Data
{
   public class Department :IEquatable<Department>
   {
      public String Name { get; private set; }
      public String Abv { get; private set; }

      public Department(string name, string abv)
      {
         Name = name;
         Abv = abv;
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as Department);
      }

      public bool Equals(Department other)
      {
         if (other == null)
            return false;
         
         return other.Name == Name && other.Abv == Abv;
      }

      int hash = -1;

      public override int GetHashCode()
      {
         if (hash == -1)
            hash = (Name + Abv).GetHashCode();
         return hash;
      }

      public override string ToString()
      {
         return Abv;
      }
   }
}
