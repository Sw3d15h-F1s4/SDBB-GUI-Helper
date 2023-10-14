using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBBGuiHelper.GUI
{
    internal class GuiRequirement
    {
        private List<GuiReqItem> RequirementItems;
        private List<String> DenyCommands;

        public GuiRequirement() //TODO: add more functionalty to make this not seem useles.
        {
            RequirementItems = new List<GuiReqItem>();
            DenyCommands = new List<String>();
        }

        public void AddReqItem(GuiReqItem item) { RequirementItems.Add(item); }
        public void AddDenyCommand(String item) { DenyCommands.Add(item);}
        public void PrintRequirements(StreamWriter file, int tabLevel)
        {
            file.WriteLine(IndentHandler.WriteTabbed("requirements:", tabLevel));
            foreach (GuiReqItem item in RequirementItems)
            {
                file.WriteLine(IndentHandler.WriteTabbed(item.Name + ":", tabLevel + 1));
                file.WriteLine(IndentHandler.WriteTabbed("type: '" + item.Type + "'", tabLevel + 2));
                foreach (String extra in item.Extra)
                {
                    file.WriteLine(IndentHandler.WriteTabbed(extra, tabLevel + 2)); //lazy
                }
            }
            if (DenyCommands.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed("deny_commands:", tabLevel));
                foreach (String command in DenyCommands)
                {
                    file.WriteLine(IndentHandler.WriteTabbed("- '" + command + "'", tabLevel + 2));
                }
            }
        }
    }
}
