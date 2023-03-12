namespace MyServicesLibrary.Controls.UniversalDataGrid;
public interface IDataSource
{
    List<IDataSource> GetSourceList(string dbpath, string sql);
}
