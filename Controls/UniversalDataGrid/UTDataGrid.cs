using MyServicesLibrary.Helpers;
using System.Collections.ObjectModel;
using DataGridTextColumn = System.Windows.Controls.DataGridTextColumn;

namespace MyServicesLibrary.Controls.UniversalDataGrid;
public partial class UTDataGrid<T> : INotifyPropertyChanged
{
    public UTDataGridControl udg;

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

    public UTDataGrid(List<T> source, List<DataGridStruct> structList)
    {
        TableData = new(source);
        udg = new();
        udg.DataContext = this;
        tableStruct = structList;
        SetGroupPanelEnable();
        udg.myDataGrid.AutoGeneratingColumn += myDataGrid_AutoGeneratingColumn;
        TableFontSizeList = new List<double> { 10, 11, 12, 13, 14, 16, 18, 20, 22 };
    }

    #region TableStructureMade
    private List<DataGridStruct> tableStruct;

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

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}