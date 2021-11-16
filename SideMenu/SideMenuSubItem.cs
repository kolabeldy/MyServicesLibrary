namespace MyServicesLibrary.SideMenu;
public class SideMenuSubItem
{
    public SideMenuSubItem(string name, string idFunc = null)
    {
        Name = name;
        IdFunc = idFunc;
    }
    public string? Name { get; private set; }
    public string? IdFunc { get; set; }
}
