using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBBGuiHelper.GUI
{
    internal class IndentHandler
    {
        public static String WriteTabbed(string text, int tabLevel = 0)
        {
            if (tabLevel == 0)
            {
                return text;
            }
            StringBuilder sb = new();
            for (int i = 0; i < tabLevel; i++)
            {
                sb.Append("  ");
            }
            sb.Append(text);
            return sb.ToString();
        }
    }
}
