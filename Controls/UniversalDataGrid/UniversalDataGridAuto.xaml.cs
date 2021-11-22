using DataGridTextColumn = System.Windows.Controls.DataGridTextColumn;

namespace MyServicesLibrary.Controls.UniversalDataGrid;
public partial class UniversalDataGrid : UserControl
{
    private List<DataGridStruct> tableStruct;
    public UniversalDataGrid(List<DataGridStruct> tableStructure)
    {
        tableStruct = tableStructure;
        InitializeComponent();
        myDataGrid.AutoGeneratingColumn += myDataGrid_AutoGeneratingColumn;
    }
    public void Show<T>(List<T> tableData)
    {
        myDataGrid.ItemsSource = tableData;
    }
    void myDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        DataGridStruct rec = tableStruct.Find(m => m.Binding == e.PropertyName);
        if (rec == null)
            e.Cancel = true;
        else
        {
            e.Column.Header = rec.Headers;
            e.Column.Width = new DataGridLength(rec.ColWidth, DataGridLengthUnitType.Star);
            if (e.PropertyType != typeof(double))
            {
                (e.Column).HeaderStyle = this.Resources["mdDataGridTextColumnHeaderStyleLeft"] as Style;
            }
            if (e.PropertyType == typeof(double))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = rec.NumericFormat;
                (e.Column as DataGridTextColumn).ElementStyle = this.Resources["mdDataGridTextColumnStyle"] as Style;
                (e.Column).HeaderStyle = this.Resources["mdDataGridTextColumnHeaderStyleRight"] as Style;
            }

        }
    }
}