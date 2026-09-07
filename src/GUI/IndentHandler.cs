using System.Text;

namespace GUI;

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
