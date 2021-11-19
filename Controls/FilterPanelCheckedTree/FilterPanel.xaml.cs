namespace MyServicesLibrary.Controls.FilterPanelCheckedTree;
public partial class FilterPanel : UserControl
{
    public FilterPanelViewModel ViewModel { get; set; }
    public FilterPanel(List<TreeFilterCollection> treeFilterCollections)
    {
        ViewModel = new FilterPanelViewModel(treeFilterCollections);
        DataContext = ViewModel;
        InitializeComponent();
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        FilterSection filterSection = (sender as ListBox).SelectedItem as FilterSection;
        filterSection.PopupBox.IsPopupOpen = true;
        //filterSection.model.IsFilterPopupOpen = true;
    }
}