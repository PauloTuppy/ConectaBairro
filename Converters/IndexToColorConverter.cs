using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace ConectaBairro.Converters;

public class IndexToColorConverter : IValueConverter
{
    public SolidColorBrush ActiveColor { get; set; } = new SolidColorBrush(Colors.Black);
    public SolidColorBrush InactiveColor { get; set; } = new SolidColorBrush(Colors.LightGray);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var currentIndex = value is int i ? i : 0;
        var itemIndex = parameter is string s && int.TryParse(s, out var p) ? p : -1;

        return currentIndex == itemIndex ? ActiveColor : InactiveColor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
