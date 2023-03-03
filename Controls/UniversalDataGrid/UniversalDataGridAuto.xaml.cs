using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using DataGridTextColumn = System.Windows.Controls.DataGridTextColumn;

namespace MyServicesLibrary.Controls.UniversalDataGrid;

public partial class UniversalDataGrid : UserControl, INotifyPropertyChanged
{
    private bool _IsTableReadOnly = true;
    public bool IsTableReadOnly
    {
        get => _IsTableReadOnly;
        set
        {
            _IsTableReadOnly = value;
            BtnTableChanged.IsEnabled = !value;
            OnPropertyChanged("IsTableReadOnly");
        }
    }

    private DbContext db;
    public void Show<T>(List<T> tableData, DbContext db = null)
    {
        myDataGrid.ItemsSource = tableData;
        this.db = db;
    }

    public UniversalDataGrid(List<DataGridStruct> tableStructure)
    {
        tableStruct = tableStructure;
        DataContext = this;
        InitializeComponent();
        BtnTableChanged.IsEnabled = !IsTableReadOnly;
        SetGroupPanelVisible();
        myDataGrid.AutoGeneratingColumn += myDataGrid_AutoGeneratingColumn;
        BtnGroupingVisibleChange();
    }
    private string _CaptionGroupPanel = "Нет";
    public string CaptionGroupPanel
    {
        get => _CaptionGroupPanel;
        set
        {
            _CaptionGroupPanel = value;
            OnPropertyChanged("CaptionGroupPanel");
        }
    }




    private List<DataGridStruct> tableStruct;
    private string groupFieldName;
    private bool isGrouping;

    private void SetGroupPanelVisible()
    {
        DataGridStruct rec = tableStruct.Find(m => m.IsGrouping == true);
        if (rec != null)
        {
            groupFieldName = rec.Binding;
            CaptionGroupPanel = rec.Headers;
            GroupPanel.Visibility = Visibility.Visible;
        }
        else
        {
            GroupPanel.Visibility = Visibility.Collapsed;
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
                (e.Column).HeaderStyle = this.Resources["mdDataGridTextColumnHeaderStyleLeft"] as Style;
            }
            else if (e.PropertyType == typeof(double))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = rec.NumericFormat;
                (e.Column as DataGridTextColumn).ElementStyle = this.Resources["mdDataGridTextColumnStyle"] as Style;
                (e.Column).HeaderStyle = this.Resources["mdDataGridTextColumnHeaderStyleRight"] as Style;
            }
        }
    }

    private void UngroupButton_Click(object sender, RoutedEventArgs e)
    {
        isGrouping = false;
        BtnGroupingVisibleChange();
        ICollectionView cvData = CollectionViewSource.GetDefaultView(myDataGrid.ItemsSource);
        cvData.SortDescriptions.Clear();
        if (cvData != null)
        {
            cvData.GroupDescriptions.Clear();
        }
    }

    private void GroupButton_Click(object sender, RoutedEventArgs e)
    {
        isGrouping = true;
        BtnGroupingVisibleChange();
        ICollectionView cvData = CollectionViewSource.GetDefaultView(myDataGrid.ItemsSource);
        cvData.SortDescriptions.Clear();
        cvData.SortDescriptions.Add(new SortDescription(groupFieldName, ListSortDirection.Ascending));
        if (cvData != null && cvData.CanGroup == true)
        {
            cvData.GroupDescriptions.Clear();
            cvData.GroupDescriptions.Add(new PropertyGroupDescription(groupFieldName));
        }
    }

    private void BtnGroupingVisibleChange()
    {
        if (isGrouping)
        {
            BtnGroup.Visibility = Visibility.Collapsed;
            BtnUnGroup.Visibility = Visibility.Visible;
        }
        else
        {
            BtnGroup.Visibility = Visibility.Visible;
            BtnUnGroup.Visibility = Visibility.Collapsed;
        }
    }

    private void BtnTableChanged_Click(object sender, RoutedEventArgs e)
    {
        db.SaveChanges();
    }


    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }

}