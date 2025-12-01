using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class ProfilePage : Page
{
    public ProfilePage()
    {
        InitializeComponent();
        LoadUserProfile();
    }

    private void LoadUserProfile()
    {
        var oauth = OAuthService.Instance;
        if (oauth.IsAuthenticated && oauth.CurrentUser != null)
        {
            var user = oauth.CurrentUser;
            UserNameText.Text = user.Name;
            UserEmailText.Text = user.Email;
            
            // Iniciais do avatar
            var initials = string.Join("", user.Name.Split(' ')
                .Where(n => !string.IsNullOrEmpty(n))
                .Take(2)
                .Select(n => n[0].ToString().ToUpper()));
            AvatarInitials.Text = string.IsNullOrEmpty(initials) ? "U" : initials;
            
            // Badge do provider
            ProviderBadge.Visibility = Visibility.Visible;
            ProviderText.Text = $"✓ {user.Provider}";
            ProviderBadge.Background = user.Provider == "Google" 
                ? new Microsoft.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 234, 67, 53))
                : new Microsoft.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 36, 41, 46));
        }
    }

    private async void Logout_Click(object sender, RoutedEventArgs e)
    {
        await OAuthService.Instance.LogoutAsync();
        NavigationService.NavigateTo<LoginPage>();
    }

    private void NavigateToHome(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<DashboardPage>();
    private void NavigateToCourses(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<CoursesPage>();
    private void NavigateToMap(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<MapPage>();
    private void NavigateToBadges(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<BadgesPage>();
    private void NavigateToProfile(object sender, RoutedEventArgs e) { }
    private void NavigateToNotifications(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<NotificationsPage>();
    private void NavigateToMessages(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<MessagesPage>();
    private void GoBack(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    private void EditProfile_Click(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<EditProfilePage>();
}
