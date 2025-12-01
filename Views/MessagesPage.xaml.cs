using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class MessagesPage : Page
{
    public MessagesPage()
    {
        InitializeComponent();
    }

    private void GoBack(object sender, RoutedEventArgs e) => NavigationService.GoBack();
}
