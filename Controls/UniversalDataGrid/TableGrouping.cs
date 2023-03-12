using MyServicesLibrary.Helpers;

namespace MyServicesLibrary.Controls.UniversalDataGrid;

public partial class UTDataGrid<T> : INotifyPropertyChanged
{
    const string GroupToolTipStr = "Группировать по:  ";
    private string groupFieldName;
    private string groupHeader;

    private int _GroupSelected = 1;
    public int GroupSelected
    {
        get => _GroupSelected;
        set
        {
            _GroupSelected = value;
            Grouping(value);
            OnPropertyChanged("GroupSelected");
        }
    }


    private string _GroupToolTip;
    public string GroupToolTip
    {
        get => _GroupToolTip;
        set
        {
            _GroupToolTip = value;
            OnPropertyChanged("GroupToolTip");
        }
    }

    private void SetGroupPanelEnable()
    {
        DataGridStruct rec = tableStruct.Find(m => m.IsGrouping == true);
        if (rec != null)
        {
            groupFieldName = rec.Binding;
            groupHeader = rec.Headers;
            udg.GroupPanel.IsEnabled = true;
            GroupToolTip = GroupToolTipStr + groupHeader;
        }
        else
        {
            udg.GroupPanel.IsEnabled = false;
        }
    }

    private void Grouping(int param)
    {
        if (!Convert.ToBoolean(param))
        {
            ICollectionView cvData = CollectionViewSource.GetDefaultView(udg.myDataGrid.ItemsSource);
            cvData.SortDescriptions.Clear();
            cvData.SortDescriptions.Add(new SortDescription(groupFieldName, ListSortDirection.Ascending));
            if (cvData != null && cvData.CanGroup == true)
            {
                cvData.GroupDescriptions.Clear();
                cvData.GroupDescriptions.Add(new PropertyGroupDescription(groupFieldName));
            }
        }
        else
        {
            ICollectionView cvData = CollectionViewSource.GetDefaultView(udg.myDataGrid.ItemsSource);
            cvData.SortDescriptions.Clear();
            if (cvData != null)
            {
                cvData.GroupDescriptions.Clear();
            }
        }
    }

    private RelayCommand groupingCommand;
    public RelayCommand GroupingCommand
    {
        get
        {
            return groupingCommand ??
                (groupingCommand = new RelayCommand(obj =>
                {
                    ICollectionView cvData = CollectionViewSource.GetDefaultView(udg.myDataGrid.ItemsSource);
                    cvData.SortDescriptions.Clear();
                    cvData.SortDescriptions.Add(new SortDescription(groupFieldName, ListSortDirection.Ascending));
                    if (cvData != null && cvData.CanGroup == true)
                    {
                        cvData.GroupDescriptions.Clear();
                        cvData.GroupDescriptions.Add(new PropertyGroupDescription(groupFieldName));
                    }
                }));
        }
    }

    private RelayCommand ungroupingCommand;
    public RelayCommand UngroupingCommand
    {
        get
        {
            return ungroupingCommand ??
                (ungroupingCommand = new RelayCommand(obj =>
                {
                    ICollectionView cvData = CollectionViewSource.GetDefaultView(udg.myDataGrid.ItemsSource);
                    cvData.SortDescriptions.Clear();
                    if (cvData != null)
                    {
                        cvData.GroupDescriptions.Clear();
                    }
                }));
        }
    }

}