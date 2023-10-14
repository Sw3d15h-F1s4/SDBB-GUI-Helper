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
            this.Name = name;
            this.Type = type;
            this.Extra = new();
        }

        public void AddExtraInfo(string extraInfo) { Extra.Add(extraInfo); }

    }
}
