using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBBGuiHelper.GUI
{
    internal class GuiRequirement
    {
        public readonly List<GuiReqItem> RequirementItems;
        public readonly List<GuiAction> DenyCommands;
        public readonly int MinimumRequirements;
        public readonly bool StopAtSuccess;

        public GuiRequirement() //TODO: add more functionalty to make this not seem useles.
        {
            RequirementItems = new();
            DenyCommands = new();
            StopAtSuccess = false;
            MinimumRequirements = 1;
        }

        public void PrintRequirements(StreamWriter file, int tabLevel)
        {
            file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "minimum_requirements: ", MinimumRequirements.ToString()));
            file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "stop_at_success: ", StopAtSuccess.ToString().ToLower()));

            file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "requirements:"));
            foreach (GuiReqItem item in RequirementItems)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 1, item.Name , ":"));
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 2, "type: '" , item.Type , "'"));
                foreach (var extra in item.Extra)
                {
                    file.WriteLine(IndentHandler.WriteTabbed(tabLevel + 2, extra)); //lazy
                }
            }
            if (DenyCommands.Count > 0)
            {
                file.WriteLine(IndentHandler.WriteTabbed(tabLevel, "deny_commands:"));
                foreach (var command in DenyCommands)
                {
                    command.PrintAction(file, tabLevel + 1);
                }
            }
        }
    }
}
