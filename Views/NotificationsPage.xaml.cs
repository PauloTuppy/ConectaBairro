using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class NotificationsPage : Page
{
    public NotificationsPage()
    {
        InitializeComponent();
    }

    private void GoBack(object sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}
