using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBBGuiHelper.GUI
{
    internal class GuiAction
    {
        public readonly string Action;
        public readonly string[] Arguments;

        public GuiAction ( string action, params string[] arguments)
        {
            Action = action;
            Arguments = arguments;
        }

        public void PrintAction(StreamWriter file, int tabLevel)
        {
            StringBuilder sb = new();
            sb.Append("- '");
            sb.Append(Action);
            foreach (var arg in Arguments)
            {
                sb.Append(' ');
                sb.Append(arg);
            }
            sb.Append('\'');
            file.WriteLine(IndentHandler.WriteTabbed(tabLevel, sb.ToString()));
        }
    }
}
