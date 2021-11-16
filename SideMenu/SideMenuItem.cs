namespace MyServicesLibrary.SideMenu;
public class SideMenuItem
{
    public SideMenuItem(string header, List<SideMenuSubItem> subItems, PackIconKind icon)
    {
        Header = header;
        SubItems = subItems;
        Icon = icon;
    }

    public string Header { get; private set; }
    public PackIconKind Icon { get; private set; }
    public List<SideMenuSubItem> SubItems { get; private set; }
}
