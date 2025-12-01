using Microsoft.UI.Xaml.Data;
using ConectaBairro.Models;

namespace ConectaBairro.Converters;

public class AlertTypeToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is AlertType alertType)
        {
            return alertType switch
            {
                AlertType.Emergency => "🚨",
                AlertType.Warning => "⚠️",
                AlertType.Info => "ℹ️",
                AlertType.Opportunity => "🎉",
                _ => "📢"
            };
        }
        return "📢";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
