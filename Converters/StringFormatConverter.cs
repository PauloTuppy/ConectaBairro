using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace ConectaBairro.Converters;

public class StringFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var format = parameter as string;
        if (string.IsNullOrEmpty(format))
        {
            return value;
        }

        if (!format.Contains("{0}") && !format.Contains("{1}")) // Check for basic composite format
        {
            // If it's a standard format specifier like "C2", prepend "{0:" and append "}"
            // Otherwise, assume it's a suffix and prepend "{0} "
            if (format.Length > 0 && (char.IsLetter(format[0]) || format.StartsWith("N") || format.StartsWith("P") || format.StartsWith("X"))) // Heuristic: "C2", "D", "P", "N", "X" etc.
            {
                 format = $"{{0:{format}}}";
            }
            else
            {
                format = $"{{0}} {format}"; // Add space for suffix
            }
        }
        
        return string.Format(CultureInfo.CurrentUICulture, format, value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
