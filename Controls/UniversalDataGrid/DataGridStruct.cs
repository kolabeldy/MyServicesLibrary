namespace MyServicesLibrary.Controls.UniversalDataGrid;
public class DataGridStruct
{
    public string Headers { get; set; }
    public string Binding { get; set; }
    public string ColWidth { get; set; }
    public double ColMinWidth { get; set; } = 80;
    public string NumericFormat { get; set; }
    public bool IsGrouping { get; set; }
    public bool IsVisible { get; set; } = true;

}