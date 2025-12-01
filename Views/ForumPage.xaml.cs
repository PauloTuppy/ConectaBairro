using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class ForumPage : Page
{
    public ForumPage()
    {
        InitializeComponent();
    }

    private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    private void NewThread_Click(object sender, RoutedEventArgs e) { /* TODO: Open new thread dialog */ }
    
    private void OpenAIChat_Click(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<AIChatPage>();
    
    private void Category_AppUsage_Click(object sender, RoutedEventArgs e) { /* TODO: Navigate to category */ }
    private void Category_Programs_Click(object sender, RoutedEventArgs e) { /* TODO: Navigate to category */ }
    private void Category_Courses_Click(object sender, RoutedEventArgs e) { /* TODO: Navigate to category */ }
    private void Category_Jobs_Click(object sender, RoutedEventArgs e) { /* TODO: Navigate to category */ }

    // Navigation
    private void NavigateToHome(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<DashboardPage>();
    private void NavigateToCourses(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<CoursesPage>();
    private void NavigateToBadges(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<BadgesPage>();
    private void NavigateToProfile(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<ProfilePage>();
}
