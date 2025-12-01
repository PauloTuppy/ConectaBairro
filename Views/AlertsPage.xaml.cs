using Microsoft.UI.Xaml.Controls;
using ConectaBairro.ViewModels;

namespace ConectaBairro.Views;

public sealed partial class AlertsPage : Page
{
    public AlertsViewModel ViewModel { get; }

    public AlertsPage()
    {
        this.InitializeComponent();
        ViewModel = new AlertsViewModel();
    }
}
