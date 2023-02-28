﻿using DataGridTextColumn = System.Windows.Controls.DataGridTextColumn;

namespace MyServicesLibrary.Controls.UniversalDataGrid;
public partial class UniversalDataGrid : UserControl
{
    public void Show<T>(List<T> tableData)
    {
        DataContext = this;
        myDataGrid.ItemsSource = tableData;
    }

    public UniversalDataGrid(List<DataGridStruct> tableStructure)
    {
        tableStruct = tableStructure;
        SetGroupPanelVisible();
        InitializeComponent();
        myDataGrid.AutoGeneratingColumn += myDataGrid_AutoGeneratingColumn;
        BtnGroupingVisible();
    }
    public string CaptionGroupPanel { get; set; } = "Нет";

    private List<DataGridStruct> tableStruct;
    private string groupFieldName;
    private Visibility IsGroupPanelVisible;
    private bool isGrouping;

    private void SetGroupPanelVisible()
    {
        DataGridStruct rec = tableStruct.Find(m => m.IsGrouping == true);
        if (rec != null)
        {
            groupFieldName = rec.Binding;
            CaptionGroupPanel = rec.Headers;
            IsGroupPanelVisible = Visibility.Visible;
        }
        else
        {
            IsGroupPanelVisible = Visibility.Collapsed;
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
        BtnGroupingVisible();
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
        BtnGroupingVisible();
        ICollectionView cvData = CollectionViewSource.GetDefaultView(myDataGrid.ItemsSource);
        cvData.SortDescriptions.Clear();
        cvData.SortDescriptions.Add(new SortDescription(groupFieldName, ListSortDirection.Ascending));
        if (cvData != null && cvData.CanGroup == true)
        {
            cvData.GroupDescriptions.Clear();
            cvData.GroupDescriptions.Add(new PropertyGroupDescription(groupFieldName));
        }
    }

    private void BtnGroupingVisible()
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

}