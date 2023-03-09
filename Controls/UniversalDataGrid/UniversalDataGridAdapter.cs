using Microsoft.EntityFrameworkCore;
using MyServicesLibrary.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using DataGridTextColumn = System.Windows.Controls.DataGridTextColumn;

namespace MyServicesLibrary.Controls.UniversalDataGrid;

public delegate void InChange(object o, bool isadd);
public delegate void InAdd();

public class UniversalDataGridAdapter<T> : INotifyPropertyChanged
{
    public UniversalDataGrid udg;
    public event InChange OnChange;
    public event InAdd OnAdd;

    const string GroupToolTipStr = "Группировать по:  ";


    private object _SelectedRow;
    public object SelectedRow
    {
        get => _SelectedRow;
        set
        {
            _SelectedRow = value;
            OnPropertyChanged("SelectedRow");
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



    private ObservableCollection<T> _TableData;
    public ObservableCollection<T> TableData
    {
        get => _TableData;
        set
        {
            _TableData = value;
            OnPropertyChanged("TableData");
        }
    }


    public UniversalDataGridAdapter(List<T> source, List<DataGridStruct> structList)
    {
        TableData = new(source);
        udg = new();
        udg.DataContext = this;
        udg.myDataGrid.ItemsSource = TableData;
        tableStruct = structList;
        udg.BtnTableChanged.IsEnabled = !IsTableReadOnly;
        SetGroupPanelEnable();
        udg.myDataGrid.AutoGeneratingColumn += myDataGrid_AutoGeneratingColumn;
        BtnGroupingVisibleChange();
        IsTableReadOnly = true;
    }
    public void AddNewRecord(T source)
    {
        TableData.Add(source);
    }


    #region GroupingPanel and ReadOnly
    private bool _IsTableReadOnly;
    public bool IsTableReadOnly
    {
        get => _IsTableReadOnly;
        set
        {
            _IsTableReadOnly = value;
            udg.BtnTableChanged.IsEnabled = !value;
            if(value)
            {
                udg.cbIsReadOnly.ToolTip = "Просмотр";
                TableEditMode = "Просмотр";
            }
            else 
            { 
                udg.cbIsReadOnly.ToolTip = "Правка";
                TableEditMode = "Правка";
            }
            OnPropertyChanged("IsTableReadOnly");
        }
    }

    private string _TableReadOnly;
    public string TableReadOnlyTool
    {
        get => _TableReadOnly;
        set
        {
            _TableReadOnly = value;
            OnPropertyChanged("TableReadOnlyTool");
        }
    }

    private string _TableEditMode;
    public string TableEditMode
    {
        get => _TableEditMode;
        set
        {
            _TableEditMode = value;
            OnPropertyChanged("TableEditMode");
        }
    }

    private string groupFieldName;
    private string groupHeader;
    private bool isGrouping;

    #endregion

    #region TableStructureMade
    private List<DataGridStruct> tableStruct;

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

    private void myDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        DataGridStruct rec = tableStruct.Find(m => m.Binding == e.PropertyName);

        if (rec == null || !rec.IsVisible)
            e.Cancel = true;
        else
        {
            e.Column.Header = rec.Headers;
            e.Column.MinWidth = rec.ColMinWidth;
            SetWidth();
            SetAligment();
        }

        void SetWidth()
        {
            string strWidth = rec.ColWidth.Trim();
            if (strWidth == null || strWidth.Length == 0)
            {
                strWidth = "1*";
            }
            if (strWidth.Contains("*") == true)
            {
                strWidth = strWidth.Substring(0, strWidth.Length - 1);
                e.Column.Width = new DataGridLength(Convert.ToDouble(strWidth), DataGridLengthUnitType.Star);
            }
            else
            {
                e.Column.Width = new DataGridLength(Convert.ToDouble(strWidth));
                e.Column.MinWidth = Convert.ToDouble(strWidth);
            }
        }

        void SetAligment()
        {
            if (e.PropertyType != typeof(double))
            {
                (e.Column).HeaderStyle = udg.Resources["mdDataGridTextColumnHeaderStyleLeft"] as Style;
            }
            else if (e.PropertyType == typeof(double))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = rec.NumericFormat;
                (e.Column as DataGridTextColumn).ElementStyle = udg.Resources["mdDataGridTextColumnStyle"] as Style;
                (e.Column).HeaderStyle = udg.Resources["mdDataGridTextColumnHeaderStyleRight"] as Style;
            }
        }
    }

    #endregion

    private void BtnGroupingVisibleChange()
    {
        if (isGrouping)
        {
            udg.BtnGroup.Visibility = Visibility.Collapsed;
            udg.BtnUnGroup.Visibility = Visibility.Visible;
        }
        else
        {
            udg.BtnGroup.Visibility = Visibility.Visible;
            udg.BtnUnGroup.Visibility = Visibility.Collapsed;
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
                    isGrouping = true;
                    BtnGroupingVisibleChange();
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
                    isGrouping = false;
                    BtnGroupingVisibleChange();
                    ICollectionView cvData = CollectionViewSource.GetDefaultView(udg.myDataGrid.ItemsSource);
                    cvData.SortDescriptions.Clear();
                    if (cvData != null)
                    {
                        cvData.GroupDescriptions.Clear();
                    }
                }));
        }
    }

    private RelayCommand dbAddCommand;
    public RelayCommand DbAddCommand
    {
        get
        {
            return dbAddCommand ??
                (dbAddCommand = new RelayCommand(obj =>
                {
                    isAdd = true;
                    OnAdd();
                    //udg.myDataGrid.CanUserAddRows = true;
                    udg.cbIsReadOnly.IsChecked = false;
                    udg.myDataGrid.SelectedIndex = udg.myDataGrid.Items.Count - 1;
                    udg.BtnTableAdd.IsEnabled = false;
                }));
        }
    }

    private bool isAdd = false;
    private RelayCommand dbSaveChangesCommand;
    public RelayCommand DbSaveChangesCommand
    {
        get
        {
            return dbSaveChangesCommand ??
                (dbSaveChangesCommand = new RelayCommand(obj =>
                {
                    udg.myDataGrid.CanUserAddRows = false;
                    udg.cbIsReadOnly.IsChecked = true;
                    udg.myDataGrid.SelectedIndex = udg.myDataGrid.Items.Count - 1;
                    OnChange(SelectedRow, isAdd);
                    isAdd = false;
                    udg.BtnTableAdd.IsEnabled = true;
                }));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}