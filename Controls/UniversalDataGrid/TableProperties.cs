using MyServicesLibrary.Helpers;

namespace MyServicesLibrary.Controls.UniversalDataGrid;
public partial class UTDataGrid<T> : INotifyPropertyChanged
{
    public List<Double> TableFontSizeList { get; set; }

    private string _TableCaption = "Заголовок таблицы";
    public string TableCaption
    {
        get => _TableCaption;
        set
        {
            _TableCaption = value;
            OnPropertyChanged("TableCaption");
        }
    }
    private double _TableFontSize = 14;
    public double TableFontSize
    {
        get => _TableFontSize;
        set
        {
            _TableFontSize = value;
            OnPropertyChanged("TableFontSize");
        }
    }

    private bool _IsToolBarEnabled = false;
    public bool IsToolBarEnabled
    {
        get => _IsToolBarEnabled;
        set
        {
            _IsToolBarEnabled = value;
            OnPropertyChanged("IsToolBarEnabled");
        }
    }

    public bool OnTableEdit { get; set; } = false;

    private bool _IsTableReadOnly = true;
    public bool IsTableReadOnly
    {
        get => _IsTableReadOnly;
        set
        {
            _IsTableReadOnly = value;
            OnPropertyChanged("IsTableReadOnly");
        }
    }

    private bool _IsAddDelEnabled = false;
    public bool IsAddDelEnabled
    {
        get => _IsAddDelEnabled;
        set
        {
            _IsAddDelEnabled = value;
            OnPropertyChanged("IsAddDelEnabled");
        }
    }

    private int _EditSelected = 1;
    public int EditSelected
    {
        get => _EditSelected;
        set
        {
            _EditSelected = value;
            if (value == 0)
            {
                IsTableReadOnly = false;
                IsAddDelEnabled = true;
            }
            else if (value == 1)
            {
                IsTableReadOnly = true;
                IsAddDelEnabled = false;
            }
            OnPropertyChanged("EditSelected");
        }
    }
    private RelayCommand tollBarVisibleChangeCommand;
    public RelayCommand TollBarVisibleChangeCommand
    {
        get
        {
            return tollBarVisibleChangeCommand ??
                (tollBarVisibleChangeCommand = new RelayCommand(obj =>
                {
                    IsToolBarEnabled = !IsToolBarEnabled;
                }));
        }
    }


}
