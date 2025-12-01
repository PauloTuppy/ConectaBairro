using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace ConectaBairro.Converters;

public class BooleanToBrushConverter : IValueConverter
{
    // Brush to use when value is true. Set in XAML as StaticResource.
    public SolidColorBrush TrueBrush { get; set; } = new SolidColorBrush(Microsoft.UI.Colors.LightGray);
    // Brush to use when value is false. Set in XAML as StaticResource.
    public SolidColorBrush FalseBrush { get; set; } = new SolidColorBrush(Microsoft.UI.Colors.White);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            // If parameter is a string, assume it's the key for a brush resource
            if (parameter is string brushKey && Application.Current.Resources.TryGetValue(brushKey, out object? resource))
            {
                if (resource is SolidColorBrush solidBrush)
                {
                    if (boolValue)
                    {
                        return solidBrush;
                    }
                    else
                    {
                        // Default to WhiteColorBrush if parameter is for true, and we are false
                        if (Application.Current.Resources.TryGetValue("WhiteColorBrush", out object? whiteBrushResource) && whiteBrushResource is SolidColorBrush whiteBrush)
                        {
                            return whiteBrush;
                        }
                        return new SolidColorBrush(Microsoft.UI.Colors.White);
                    }
                }
            }
            // Fallback to default brushes if resource not found or parameter not provided
            return boolValue ? TrueBrush : FalseBrush;
        }
        return FalseBrush; // Default for non-boolean values
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
