using MyServicesLibrary.Helpers;

namespace MyServicesLibrary.Controls.UniversalDataGrid;

public delegate void InChange(object o, bool isadd);
public delegate void InAdd();

public partial class UTDataGrid<T> : INotifyPropertyChanged
{
    public event InChange OnChange;
    public event InAdd OnAdd;

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

    public void AddNewRecord(T source)
    {
        TableData.Add(source);
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
                    udg.myDataGrid.SelectedIndex = udg.myDataGrid.Items.Count - 1;
                    //udg.BtnTableAdd.IsEnabled = false;
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
                    udg.myDataGrid.SelectedIndex = udg.myDataGrid.Items.Count - 1;
                    OnChange(SelectedRow, isAdd);
                    isAdd = false;
                    //udg.BtnTableAdd.IsEnabled = true;
                }));
        }
    }

}