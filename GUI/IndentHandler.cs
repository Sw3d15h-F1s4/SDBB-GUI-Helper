using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBBGuiHelper.GUI
{
    internal class IndentHandler
    {
        public static string WriteTabbed(int tabLevel, params string[] text)
        {
            StringBuilder sb = new();
            for (int i = 0; i < tabLevel; i++)
            {
                sb.Append("  ");
            }
            foreach (var item in text)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }
    }
}
