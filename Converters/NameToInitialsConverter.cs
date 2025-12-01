using Microsoft.UI.Xaml.Data;
using System;
using System.Linq;

namespace ConectaBairro.Converters;

public class NameToInitialsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not string name || string.IsNullOrEmpty(name))
        {
            return "??";
        }

        var parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
        {
            return parts[0].Length > 1 ? parts[0].Substring(0, 2).ToUpper() : parts[0].ToUpper();
        }

        return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpper();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
