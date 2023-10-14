using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SDBBGuiHelper.GUI
{
    internal class GuiItem
    {
        public String ItemName
        { get; private set; }
        private readonly String DisplayName;
        private readonly String Material;
        public int Slot { get; private set; }
        private List<String> Lore;
        private readonly int Priority; //0 is highest priority
        private List<GuiRequirement> ViewRequirements;
        private List<GuiRequirement> ClickRequirements;
        private List<String> ClickCommands;

        public GuiItem(String item_name, String display_name, String material, int slot, int priority = 0)
        {
            this.ItemName = item_name;
            this.DisplayName = display_name;
            this.Material = material;
            this.Slot = slot;
            this.Priority = priority;

            this.Lore = new();
            this.ViewRequirements = new();
            this.ClickRequirements = new();
            this.ClickCommands = new();
        }

        public void AddLore(String lore) { Lore.Add(lore); }
        public void RemoveLore(String lore) {  Lore.Remove(lore);}

        public void AddViewRequirement(GuiRequirement viewRequirement) { ViewRequirements.Add(viewRequirement);}
        public void RemoveViewRequirement(GuiRequirement viewRequirement) { ViewRequirements.Remove(viewRequirement);}

        public void AddClickRequirement(GuiRequirement clickRequirement) { ClickRequirements.Add(clickRequirement);}
        public void RemoveClickRequirement(GuiRequirement clickRequirement) { ClickRequirements.Remove(clickRequirement);}

        public void AddClickCommand(String clickCommand) { ClickCommands.Add(clickCommand);}
        public void RemoveClickCommand(String clickCommand) { ClickCommands.Remove(clickCommand);}
        
        public void PrintItem(StreamWriter file, int tabLevel = 1)
        {
            file.WriteLine(IndentHandler.WriteTabbed("'" + this.ItemName + "':", tabLevel));

            tabLevel++;
            
            file.Write(IndentHandler.WriteTabbed("material: ", tabLevel));
            file.WriteLine(this.Material);

            file.Write(IndentHandler.WriteTabbed("display_name: ", tabLevel));
            file.WriteLine("'" + this.DisplayName + "'");
            
            file.Write(IndentHandler.WriteTabbed("slot: ", tabLevel));
            file.WriteLine(this.Slot.ToString());

            file.Write(IndentHandler.WriteTabbed("priority: ", tabLevel));
            file.WriteLine(this.Priority.ToString());

            file.WriteLine(IndentHandler.WriteTabbed("lore: ", tabLevel));
            foreach (var loreLine in Lore)
            {
                file.WriteLine(IndentHandler.WriteTabbed("- '"+ loreLine + "'", tabLevel + 1));
            }

            file.WriteLine(IndentHandler.WriteTabbed("view_requirement:", tabLevel));
            foreach (var viewRequirement in ViewRequirements)
            {
                viewRequirement.PrintRequirements(file, tabLevel + 1);
            }

            file.WriteLine(IndentHandler.WriteTabbed("click_requirement:", tabLevel));
            foreach (var clickRequirement in ClickRequirements)
            {
                clickRequirement.PrintRequirements(file, tabLevel + 1);
            }

            file.WriteLine(IndentHandler.WriteTabbed("click_commands:", tabLevel));
            foreach (var clickCommand in ClickCommands)
            {
                file.WriteLine(IndentHandler.WriteTabbed("- '" + clickCommand + "'", tabLevel + 1));
            }

        }
    }
}
