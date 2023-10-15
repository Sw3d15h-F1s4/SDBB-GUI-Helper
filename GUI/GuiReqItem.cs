using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBBGuiHelper.GUI
{
    internal class GuiReqItem
    {
        public string Name;
        public string Type;
        public List<String> Extra;

        public GuiReqItem(string name, string type) {
            Name = name;
            Type = type;
            Extra = new(); //i dont see a good reason to waste hours adding every type of requirement, yet.
        }

    }
}
