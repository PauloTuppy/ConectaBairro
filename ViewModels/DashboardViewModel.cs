using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ConectaBairro.Models; // For Alert model
using ConectaBairro.Services; // For DatabaseService
using CommunityToolkit.Mvvm.Input; // For RelayCommand
using System.Linq; // For LINQ operations

namespace ConectaBairro.ViewModels;

public class MapLayer
{
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "";
    public bool IsActive { get; set; }
}

public class FloatingCard
{
    public string ImageSource { get; set; } = "";
    public string Description { get; set; } = "";
    public string ButtonText { get; set; } = "";
}

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<MapLayer> mapLayers = new();

    [ObservableProperty]
    private ObservableCollection<FloatingCard> floatingCards = new();

    [ObservableProperty]
    private ObservableCollection<Alert> alerts = new();

    private readonly DatabaseService _databaseService;

    public DashboardViewModel()
    {
        _databaseService = DatabaseService.Instance.Result; // Get the singleton instance

        // Placeholder Data for Map Layers
        MapLayers.Add(new MapLayer { Name = "Recursos Públicos", Icon = "🟢", IsActive = true });
        MapLayers.Add(new MapLayer { Name = "Programas Sociais", Icon = "🔵", IsActive = false });
        MapLayers.Add(new MapLayer { Name = "Economia Circular", Icon = "🟠", IsActive = false });

        // Placeholder Data for Floating Cards
        FloatingCards.Add(new FloatingCard { ImageSource = "ms-appx:///Assets/placeholder_social_program.png", Description = "3 vagas abertas esta semana no Autonomia e Renda", ButtonText = "Descobrir" });
        FloatingCards.Add(new FloatingCard { ImageSource = "ms-appx:///Assets/placeholder_event.png", Description = "Evento comunitário: Limpeza do parque no sábado", ButtonText = "Ver Detalhes" });
        
        LoadAlerts();
    }

    private async void LoadAlerts()
    {
        var loadedAlerts = await _databaseService.GetItemsAsync<Alert>();
        Alerts = new ObservableCollection<Alert>(loadedAlerts.Where(a => a.IsActive).OrderByDescending(a => a.Timestamp));
    }

    [RelayCommand]
    private async Task DismissAlert(Alert alert)
    {
        if (Alerts.Contains(alert))
        {
            // Create a new record with IsActive set to false (records are immutable)
            var updatedAlert = alert with { IsActive = false }; 
            await _databaseService.SaveItemAsync(updatedAlert);
            Alerts.Remove(alert);
        }
    }
}
