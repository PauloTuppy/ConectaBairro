using Microsoft.UI.Xaml.Data;
using ConectaBairro.Models;
using Windows.UI;

namespace ConectaBairro.Converters;

public class AlertTypeToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is AlertType alertType)
        {
            return alertType switch
            {
                AlertType.Emergency => Color.FromArgb(255, 211, 47, 47),  // #D32F2F
                AlertType.Warning => Color.FromArgb(255, 255, 152, 0),    // #FF9800
                AlertType.Info => Color.FromArgb(255, 0, 120, 212),       // #0078D4
                AlertType.Opportunity => Color.FromArgb(255, 16, 124, 16), // #107C10
                _ => Color.FromArgb(255, 117, 117, 117)                   // #757575
            };
        }
        return Color.FromArgb(255, 117, 117, 117);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
