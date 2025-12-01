using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class OpportunitiesPage : Page
{
    public OpportunitiesPage()
    {
        InitializeComponent();
    }

    private void NavigateToHome(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<DashboardPage>();
    private void NavigateToCourses(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<CoursesPage>();
    private void NavigateToBadges(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<BadgesPage>();
    private void NavigateToProfile(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<ProfilePage>();
}
