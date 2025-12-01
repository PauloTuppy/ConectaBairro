using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;
using System.Globalization;

namespace ConectaBairro.Views;

public sealed partial class DashboardPage : Page
{
    private const double DefaultLat = -23.5874; // Vila Mariana
    private const double DefaultLng = -46.6388;
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    public DashboardPage()
    {
        InitializeComponent();
        Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        await LoadMapAsync();
    }

    private async System.Threading.Tasks.Task LoadMapAsync()
    {
        try
        {
            await MapWebView.EnsureCoreWebView2Async();
            
            // Navigate directly to Google Maps Embed URL
            var embedUrl = $"https://www.google.com/maps/embed/v1/place?key={GoogleMapsApiKey}&q={F(DefaultLat)},{F(DefaultLng)}&zoom=15";
            MapWebView.Source = new System.Uri(embedUrl);
            
            MapWebView.NavigationCompleted += (s, args) =>
            {
                MapLoadingOverlay.Visibility = Visibility.Collapsed;
            };
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Map error: {ex.Message}");
            MapLoadingOverlay.Visibility = Visibility.Collapsed;
        }
    }

    private static string F(double v) => v.ToString(Inv);

    private const string GoogleMapsApiKey = "AIzaSyDmV_dwbLnuJGCFLvXnaKlrz0g7rzrUM1Q";

    private void NavigateToHome(object sender, RoutedEventArgs e)
    {
        // Already on home
    }

    private void NavigateToCourses(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<CoursesPage>();
    }

    private void NavigateToMap(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<MapPage>();
    }

    private void NavigateToGoogleMap(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<GoogleMapPage>();
    }

    private void NavigateToBadges(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<BadgesPage>();
    }

    private void NavigateToProfile(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<ProfilePage>();
    }

    private void NavigateToNotifications(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<NotificationsPage>();
    }

    private void NavigateToOpportunities(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<OpportunitiesPage>();
    }

    private void NavigateToForum(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<ForumPage>();
    }
}
