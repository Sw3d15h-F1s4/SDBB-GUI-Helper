namespace GUI;

internal class GuiReqItem(string name, string type)
{
    public string Name        = name;
    public string Type        = type;
    public List<string> Extra = [];
}
