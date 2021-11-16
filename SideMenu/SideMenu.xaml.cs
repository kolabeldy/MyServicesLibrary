namespace MyServicesLibrary.SideMenu;
public partial class SideMenu : UserControl
{
    public StackPanel MainMenu { get; set; }
    public event IsMenuChoise? OnMenuChoise;

    private string dbpath;

    private void MenuChoiseCommand(string methodName) => OnMenuChoise(methodName);

    private (List<string> names, List<PackIconKind> icons) GetMenuBlockProperties(List<SideMenuTable> menuDatas)
    {
        List<string> res1 = new();
        List<PackIconKind> res2 = new();
        foreach (var r in menuDatas)
            if (r.IdParent == null || r.IdParent == "")
            {
                res1.Add(r.IdMenu);
                res2.Add((PackIconKind)Enum.Parse(typeof(PackIconKind), r.IdIcon));
            }
        return (res1, res2);
    }
    private List<SideMenuTable> GetCurrentMenuBlock(List<SideMenuTable> menuDatas, string blockName)
    {
        List<SideMenuTable> res = new();
        res.AddRange(menuDatas.FindAll(m => m.IdParent == blockName));
        return res;
    }

    public SideMenu(string dbpath)
    {
        this.dbpath = dbpath;
        List<SideMenuTable> menuItems = new();
        menuItems = Get();

        MainMenu = new StackPanel();
        var menuBlockProperty = GetMenuBlockProperties(menuItems);
        List<string> blockNames = menuBlockProperty.names;
        List<PackIconKind> blockIcons = menuBlockProperty.icons;

        int blockCount = blockNames.Count;
        for (int i = 0; i < blockCount; i++)
        {
            List<SideMenuSubItem> subItem = new();
            foreach (var r in GetCurrentMenuBlock(menuItems, blockNames[i]))
                subItem.Add(new SideMenuSubItem(r.IdMenu, r.IdFunc));

            SideMenuItem menuItem = new SideMenuItem(blockNames[i], subItem, blockIcons[i]);
            UserControlMenuItem item = new(menuItem);
            item.OnMenuChoise += MenuChoiseCommand;
            MainMenu.Children.Add(item);
        }
        InitializeComponent();
        DataContext = this;
    }
    public List<SideMenuTable> Get()
    {
        
        List<SideMenuTable> result = new();

        string sql = "SELECT Parent, Name, MethodName, Icon FROM MenuTable";

        DataTable dt = new DataTable();
        dt = Sqlite.Select(dbpath, sql);
        result = (from DataRow dr in dt.Rows
                select new SideMenuTable()
                {
                    IdParent = dr["Parent"].ToString(),
                    IdMenu = dr["Name"].ToString(),
                    IdFunc = dr["MethodName"].ToString(),
                    IdIcon = dr["Icon"].ToString()
                }).ToList();
        return result;
    }

}
