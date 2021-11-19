using MyServicesLibrary.Helpers;
using MyServicesLibrary.ViewModelsBase;

namespace MyServicesLibrary.Controls.FilterPanelCheckedTree;
public class FilterPanelViewModel : BaseViewModel
{
    public delegate void IsChangeMetodContainer();
    public event IsChangeMetodContainer onChange;

    private bool _FiltersIsChanged = false;
    public bool FiltersIsChanged
    {
        get => _FiltersIsChanged;
        set
        {
            if (value)
                RefreshVisible = Visibility.Visible;
            else
                RefreshVisible = Visibility.Hidden;
            Set(ref _FiltersIsChanged, value);
        }
    }

    private Visibility _RefreshVisible = Visibility.Hidden;
    public Visibility RefreshVisible
    {
        get => _RefreshVisible;
        set
        {
            Set(ref _RefreshVisible, value);
        }
    }

    public List<FilterSection> FilterSections { get; set; } = new();
    private List<FilterSectionViewModel> _FilterSectionViewModels = new();
    public FilterPanelViewModel(List<TreeFilterCollection> treeFilterCollections)
    {
        int i = 0;
        foreach(var r in treeFilterCollections)
        {
            _FilterSectionViewModels.Add(new FilterSectionViewModel());
            _FilterSectionViewModels[i].onChange += FilterOnChangeHandler;
            _FilterSectionViewModels[i].Init(treeFilterCollections[i].Title, treeFilterCollections[i].FilterCollection, treeFilterCollections[i].InitType);
            FilterSections.Add(new FilterSection(_FilterSectionViewModels[i]));
            i++;
        }
    }

    protected RelayCommand _Refresh_Command;
    public RelayCommand Refresh_Command
    {
        get
        {
            return _Refresh_Command ??
                (_Refresh_Command = new RelayCommand(obj =>
                {
                    onChange();
                    FiltersIsChanged = false;
                }));
        }
    }

    private void FilterOnChangeHandler()
    {
        FiltersIsChanged = true;
    }

}
