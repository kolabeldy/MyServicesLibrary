namespace MyServicesLibrary.SideMenu;
public delegate void IsMenuChoise(string methodname);
public partial class UserControlMenuItem : UserControl
{
    public event IsMenuChoise OnMenuChoise;
    public UserControlMenuItem(SideMenuItem itemMenu)
    {
        InitializeComponent();
        ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
        DataContext = itemMenu;
    }

    private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListViewMenu.SelectedIndex >= 0)
        {
            string MethodName = ((SideMenuSubItem)((ListView)sender).SelectedItem).IdFunc;
            OnMenuChoise(MethodName);
            ListViewMenu.SelectedIndex = -1;

        }
    }
}
