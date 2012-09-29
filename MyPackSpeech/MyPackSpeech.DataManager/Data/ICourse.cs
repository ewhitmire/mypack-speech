using System;
namespace MyPackSpeech.DataManager.Data
{
   public interface ICourse
   {
      Department Dept { get; }
      string Name { get; }
      int Number { get; }
   }
}
