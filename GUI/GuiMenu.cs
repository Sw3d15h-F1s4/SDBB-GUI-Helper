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
        public string MenuTitle
        {
            get;
            private set;
        }
        private string OpenCommand;
        public int InventorySize { get; private set; }
        private bool RegisterCommand;
        public List<GuiItem> Items
        {
            get;
            private set;
        }

        public GuiMenu(string menu_title, string open_command, bool register = false)
        {
            MenuTitle = menu_title;
            OpenCommand = open_command;
            InventorySize = 9;
            RegisterCommand = register;
            Items = new List<GuiItem>();
        }

        public void AddItem(GuiItem item)
        {
            Items.Add(item);
            if (item.Slot > 8)
            {
                InventorySize = 18;
            }
            if (item.Slot > 17)
            {
                InventorySize = 27;
            }
            if (item.Slot > 26)
            {
                InventorySize = 36;
            }
            if (item.Slot > 35)
            {
                InventorySize = 45;
            }
            if (item.Slot > 45)
            {
                InventorySize = 54;
            }
            if (item.Slot > 53)
            {
                throw new Exception("Too many items in one menu! Attempted to index past slot 53");
            }
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

            file.Write("size: ");
            file.WriteLine(this.InventorySize);

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
