using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class CoursesPage : Page
{
    public CoursesPage()
    {
        InitializeComponent();
    }

    private void NavigateToHome(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<DashboardPage>();
    private void NavigateToCourses(object sender, RoutedEventArgs e) { }
    private void NavigateToMap(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<MapPage>();
    private void NavigateToBadges(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<BadgesPage>();
    private void NavigateToProfile(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<ProfilePage>();
}
