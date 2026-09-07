using System.Text;

namespace GUI;

internal class GuiAction(string Action, params string[] Arguments)
{
    public void PrintAction(StreamWriter file, int tabLevel)
    {
        StringBuilder sb = new();
        sb.Append("- '");
        sb.Append(Action);
        foreach (var arg in Arguments)
        {
            sb.Append(EscapeString(arg));
        }
        sb.Append('\'');
        file.WriteLine(IndentHandler.WriteTabbed(tabLevel, sb.ToString()));
    }
}
