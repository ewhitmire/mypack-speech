using System;
namespace MyPackSpeech.DataManager.Data
{
   public interface ICourse
   {
      Department Dept { get; }
      int Number { get; }
      string Name { get; }
   }
}
