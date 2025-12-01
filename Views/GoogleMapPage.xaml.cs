using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;
using System.Globalization;

namespace ConectaBairro.Views;

public sealed partial class GoogleMapPage : Page
{
    private double _currentLat = -23.5505;
    private double _currentLng = -46.6333;
    private string _currentFilter = "all";
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;
    
    private const string GoogleMapsApiKey = "AIzaSyDmV_dwbLnuJGCFLvXnaKlrz0g7rzrUM1Q";

    public GoogleMapPage()
    {
        InitializeComponent();
        Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        await InitializeMapAsync();
    }

    private async System.Threading.Tasks.Task InitializeMapAsync()
    {
        try
        {
            // Try to get real location
            var position = await MapService.Instance.GetCurrentLocationAsync();
            if (position.HasValue)
            {
                _currentLat = position.Value.Latitude;
                _currentLng = position.Value.Longitude;
                LocationText.Text = "Your current location";
            }
            else
            {
                LocationText.Text = "São Paulo, SP (default)";
            }

            // Initialize WebView2
            await MapWebView.EnsureCoreWebView2Async();
            
            // Navigate directly to Google Maps Embed URL
            var embedUrl = $"https://www.google.com/maps/embed/v1/place?key={GoogleMapsApiKey}&q={F(_currentLat)},{F(_currentLng)}&zoom=14";
            MapWebView.Source = new System.Uri(embedUrl);
            
            // Hide loading after navigation completes
            MapWebView.NavigationCompleted += (s, args) =>
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
                if (args.IsSuccess)
                {
                    LocationText.Text = "Map loaded successfully";
                }
                else
                {
                    LocationText.Text = "Tap 'Open Maps' to view";
                }
            };
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Map error: {ex.Message}");
            LocationText.Text = "Tap 'Open Maps' button";
            LoadingOverlay.Visibility = Visibility.Collapsed;
        }
    }

    private static string F(double v) => v.ToString(Inv);

    // Filters
    private void FilterAll_Click(object sender, RoutedEventArgs e) => ApplyFilter("all");
    private void FilterSaude_Click(object sender, RoutedEventArgs e) => ApplyFilter("health");
    private void FilterEducacao_Click(object sender, RoutedEventArgs e) => ApplyFilter("education");
    private void FilterSocial_Click(object sender, RoutedEventArgs e) => ApplyFilter("social");
    private void FilterTrabalho_Click(object sender, RoutedEventArgs e) => ApplyFilter("jobs");

    private void ApplyFilter(string filter)
    {
        _currentFilter = filter;
        UpdateFilterButtons();
        
        // Update map based on filter
        string searchQuery = filter switch
        {
            "health" => "hospital+clinic+UBS",
            "education" => "school+SENAI+SENAC+university",
            "social" => "CRAS+social+services",
            "jobs" => "employment+agency+SINE",
            _ => ""
        };
        
        if (!string.IsNullOrEmpty(searchQuery))
        {
            var searchUrl = $"https://www.google.com/maps/embed/v1/search?key={GoogleMapsApiKey}&q={searchQuery}&center={F(_currentLat)},{F(_currentLng)}&zoom=13";
            MapWebView.Source = new System.Uri(searchUrl);
        }
        else
        {
            var placeUrl = $"https://www.google.com/maps/embed/v1/place?key={GoogleMapsApiKey}&q={F(_currentLat)},{F(_currentLng)}&zoom=14";
            MapWebView.Source = new System.Uri(placeUrl);
        }
    }

    private void UpdateFilterButtons()
    {
        var active = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.ForestGreen);
        var inactive = new Microsoft.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 243, 244, 246));
        
        FilterAll.Background = _currentFilter == "all" ? active : inactive;
        FilterSaude.Background = _currentFilter == "health" 
            ? new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Crimson) : inactive;
        FilterEducacao.Background = _currentFilter == "education" 
            ? new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.RoyalBlue) : inactive;
        FilterSocial.Background = _currentFilter == "social" 
            ? new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.BlueViolet) : inactive;
        FilterTrabalho.Background = _currentFilter == "jobs" 
            ? new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Orange) : inactive;
    }

    // Actions - Open external Google Maps
    private async void OpenGoogleMaps_Click(object sender, RoutedEventArgs e)
    {
        var url = $"https://www.google.com/maps/@{F(_currentLat)},{F(_currentLng)},14z";
        await Windows.System.Launcher.LaunchUriAsync(new System.Uri(url));
    }

    private async void GetDirections_Click(object sender, RoutedEventArgs e)
    {
        // Directions to nearest SENAI
        var destLat = _currentLat + 0.003;
        var destLng = _currentLng - 0.004;
        var url = $"https://www.google.com/maps/dir/?api=1&destination={F(destLat)},{F(destLng)}&travelmode=transit";
        await Windows.System.Launcher.LaunchUriAsync(new System.Uri(url));
    }

    private async void NearbyPlaces_Click(object sender, RoutedEventArgs e)
    {
        // Search for public services nearby
        var url = $"https://www.google.com/maps/search/public+services/@{F(_currentLat)},{F(_currentLng)},14z";
        await Windows.System.Launcher.LaunchUriAsync(new System.Uri(url));
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<MapPage>();
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e)
    {
        LoadingOverlay.Visibility = Visibility.Visible;
        await InitializeMapAsync();
    }
}
