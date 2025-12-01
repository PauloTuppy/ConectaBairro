using ConectaBairro.ViewModels;
using Microsoft.UI.Xaml;

namespace ConectaBairro.Views;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        this.DataContext = new CourseRecommendationViewModel();
    }

    private void NavigateToAlerts(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(AlertsPage));
    }

    private void NavigateToAnimations(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(AnimationsTestPage));
    }
}
