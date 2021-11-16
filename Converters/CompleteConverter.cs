namespace MyServicesLibrary.Converters;

[ValueConversion(typeof(Boolean), typeof(String))]
public class IsNormConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        string par = parameter as string;
        string[] result = par.Split('-');
        bool isTrue = (bool)value;
        if (isTrue)
            return result[0];
        else
            return result[1];
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        //string strComplete = (string)value;
        //if (strComplete == "Нормируемые")
        //    return true;
        //else
        return false;
    }
}
