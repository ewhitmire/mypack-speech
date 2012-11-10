using MyPackSpeech.DataManager;
using MyPackSpeech.DataManager.Data;
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

namespace MyPackSpeech
{
   /// <summary>
   /// Interaction logic for RequirementsView.xaml
   /// </summary>
   public partial class RequirementsView : UserControl
   {
      public RequirementsView()
      {
         InitializeComponent();

         DegreeProgram degree = ActionManager.Instance.CurrStudent.Degree;
         if (degree != null)
         {
            foreach (DegreeRequirementCategory cat in degree.GetCategories())
            {
               TreeViewItem categoryChild = new TreeViewItem();
               categoryChild.Header = cat.Name;
               reqTree.Items.Add(categoryChild);
               IEnumerable<DegreeRequirement> reqs = degree.Requirements.Where<DegreeRequirement>(r => r.Category.Equals(cat));
               foreach (DegreeRequirement req in reqs)
               {
                  RequirementEntry reqEntry = new RequirementEntry(req);
                  categoryChild.Items.Add(reqEntry);
               }
            }
         }
      }
   }
}
