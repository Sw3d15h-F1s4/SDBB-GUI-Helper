using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SDBBGuiHelper.GUI
{
    internal class GuiMenu
    {
        public readonly string MenuTitle;                                    // Title of the menu.
        public readonly string OpenCommand;                                  // Command to open menu. Must be unique.
        public readonly bool RegisterCommand;                                // True if you want the OpenCommand to appear on the client's end.
        public readonly List<GuiAction> OpenCommands;                        // Commands run when the menu is opened.
        public readonly List<GuiAction> CloseCommands;                       // Commands run ONLY when [close] action is sent.
        public readonly List<string> Args;                                   // Optional arguments to use in the menu.
        public string? ArgsUsageMessage;                            // Tab completion for the client.
        public int? UpdateInterval;                                 // How often items are able to update.

        public readonly GuiRequirement OpenRequirement;             // Requirements to view the menu.
        public readonly List<GuiItem> Items;                        // The list of items in the menu.
        public InventorySizes InventorySize;                        // Size of the inventory. Defaults to 9, adjust automagically.
        public readonly string InventoryType;                       // Should be an InventoryTypes
        public enum InventorySizes : int
        {
            ONE_ROW = 9,
            TWO_ROWS = 18,
            THREE_ROWS = 27,
            FOUR_ROWS = 36,
            FIVE_ROWS = 45,
            SIX_ROWS = 54,
        }

        public GuiMenu(string menu_title, string open_command, bool register = false)
        {
            MenuTitle = menu_title;
            OpenCommand = open_command;
            InventoryType = InventoryTypes.Chest;
            InventorySize = InventorySizes.ONE_ROW;
            RegisterCommand = register;

            Items = new();
            OpenRequirement = new();
            OpenCommands = new();
            CloseCommands = new();
            Args = new();
        }


        public bool AddItem(GuiItem item)
        {
            if (InventoryType != InventoryTypes.Chest)
            {
                Items.Add(item);
                return true;
            }
            if (item.Slot > 8)
            {
                InventorySize = InventorySizes.ONE_ROW;
            }
            if (item.Slot > 17)
            {
                InventorySize = InventorySizes.TWO_ROWS;
            }
            if (item.Slot > 26)
            {
                InventorySize = InventorySizes.THREE_ROWS;
            }
            if (item.Slot > 35)
            {
                InventorySize = InventorySizes.FOUR_ROWS;
            }
            if (item.Slot > 45)
            {
                InventorySize = InventorySizes.FIVE_ROWS;
            }
            if (item.Slot > 53)
            {
                //throw new Exception("Too many items in one menu! Attempted to index past slot 53");
                return false;
            }
            Items.Add(item);
            return true;
        }

        public void RemoveItem(GuiItem item)
        {
            if (this.Items.Contains(item))
            {
                this.Items.Remove(item);
            }
        }


        public void PrintMenu(StreamWriter file)
        {
            file.Write("menu_title: ");
            file.WriteLine("'" + this.MenuTitle + "'");

            file.Write("open_command: ");
            file.WriteLine(this.OpenCommand);

            file.Write("register_command: ");
            file.WriteLine(RegisterCommand.ToString().ToLower());

            if (InventoryType != InventoryTypes.Chest)
            {
                file.Write("inventory_type: ");
                file.WriteLine(InventoryType);
            }

            if (InventoryType == InventoryTypes.Chest)
            {
                file.Write("size: ");
                file.WriteLine((int)InventorySize);
            }

            if (OpenRequirement.RequirementItems.Count > 0)
            {
                file.WriteLine("open_requirement:");
                OpenRequirement.PrintRequirements(file, 1);
            }

            if (OpenCommands.Count > 0)
            {
                file.WriteLine("open_commands:");
                foreach (var command in OpenCommands)
                {
                    command.PrintAction(file, 1);
                }
            }

            if (CloseCommands.Count > 0)
            {
                file.WriteLine("close_commands:");
                foreach (var command in CloseCommands)
                {
                    command.PrintAction(file, 1);
                }
            }

            if (Args.Count > 0)
            {
                file.WriteLine("args:");
                foreach (var arg in Args)
                {
                    file.WriteLine(IndentHandler.WriteTabbed(1, "- ", arg));
                }
            }

            if (ArgsUsageMessage != null)
            {
                file.Write("args_usage_message: ");
                file.WriteLine(ArgsUsageMessage);
            }


            file.WriteLine("items: ");
            foreach (GuiItem item in Items)
            {
                item.PrintItem(file, 1);
                file.WriteLine();
            }
            file.Close();
        }
    }
}
