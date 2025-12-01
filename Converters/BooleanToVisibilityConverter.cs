using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace ConectaBairro.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var boolValue = value is bool b && b;
        if (parameter is string str && str.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
        {
            boolValue = !boolValue;
        }
        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
