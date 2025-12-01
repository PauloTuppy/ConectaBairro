using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;
using System;

namespace ConectaBairro.Views;

public sealed partial class MapPage : Page
{
    // Coordenadas de São Paulo
    private const double DefaultLat = -23.5505;
    private const double DefaultLng = -46.6333;

    public MapPage()
    {
        InitializeComponent();
    }

    // Abre mapa completo com todos os recursos
    private async void OpenFullMap_Click(object sender, RoutedEventArgs e)
    {
        var url = $"https://www.google.com/maps/search/serviços+públicos/@{DefaultLat},{DefaultLng},14z";
        await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
    }

    // Busca por categoria
    private async void OpenSaude_Click(object sender, RoutedEventArgs e)
    {
        var url = $"https://www.google.com/maps/search/UBS+hospital+UPA/@{DefaultLat},{DefaultLng},13z";
        await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
    }

    private async void OpenEducacao_Click(object sender, RoutedEventArgs e)
    {
        var url = $"https://www.google.com/maps/search/SENAI+SENAC+ETEC+escola+técnica/@{DefaultLat},{DefaultLng},12z";
        await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
    }

    private async void OpenSocial_Click(object sender, RoutedEventArgs e)
    {
        var url = $"https://www.google.com/maps/search/CRAS+CREAS+assistência+social/@{DefaultLat},{DefaultLng},13z";
        await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
    }

    private async void OpenTrabalho_Click(object sender, RoutedEventArgs e)
    {
        var url = $"https://www.google.com/maps/search/SINE+CAT+Poupatempo/@{DefaultLat},{DefaultLng},13z";
        await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
    }

    private async void OpenAutonomia_Click(object sender, RoutedEventArgs e)
    {
        await AutonomiaRendaService.AbrirSiteAutonomiaRendaAsync();
    }

    private async void OpenCNCT_Click(object sender, RoutedEventArgs e)
    {
        await Windows.System.Launcher.LaunchUriAsync(new Uri("https://cnct.mec.gov.br"));
    }

    // Abre mapa integrado com WebView
    private void OpenIntegratedMap_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.NavigateTo<GoogleMapPage>();
    }

    // Navigation
    private void NavigateToHome(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<DashboardPage>();
    private void NavigateToCourses(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<CoursesPage>();
    private void NavigateToBadges(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<BadgesPage>();
    private void NavigateToProfile(object sender, RoutedEventArgs e) => NavigationService.NavigateTo<ProfilePage>();
}
