using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConectaBairro.Services;
using System.Collections.ObjectModel;

namespace ConectaBairro.ViewModels;

public class Resource
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Type { get; set; } = ""; // "Saúde", "Programas", etc
    public string Address { get; set; } = "";
    public string Icon { get; set; } = ""; // Emoji
    public double Distance { get; set; } // km
    public string Hours { get; set; } = ""; // "08:00-17:00"
    public bool IsOpen { get; set; }
    public string Details { get; set; } = "";
}

public partial class ResourceMapViewModel : ObservableObject
{
    private readonly BrasilAPIService _brasilApi = new();

    [ObservableProperty]
    private string searchQuery = "";

    [ObservableProperty]
    private string currentLocation = "São Paulo, SP";

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string selectedFilter = "Todas";

    public ObservableCollection<Resource> AllResources { get; } = new();
    public ObservableCollection<Resource> FilteredResources { get; } = new();

    public ResourceMapViewModel()
    {
        LoadMockResources();
        ApplyFilter();
    }

    private void LoadMockResources()
    {
        AllResources.Add(new Resource { Name = "UBS Bela Vista", Type = "Saúde", Address = "Rua da Saúde, 123", Icon = "🏥", Distance = 0.5, Hours = "07:00-19:00", IsOpen = true });
        AllResources.Add(new Resource { Name = "CRAS Vila Nova", Type = "Programas Sociais", Address = "Av. Social, 456", Icon = "💼", Distance = 1.2, Hours = "08:00-17:00", IsOpen = true });
        AllResources.Add(new Resource { Name = "Biblioteca Comunitária", Type = "Educação", Address = "Praça do Saber, 78", Icon = "📚", Distance = 0.3, Hours = "09:00-18:00", IsOpen = true });
        AllResources.Add(new Resource { Name = "Grupo de Economia Circular", Type = "Economia Circular", Address = "Rua Verde, 90", Icon = "🤝", Distance = 2.1, Hours = "10:00-16:00", IsOpen = false });
        AllResources.Add(new Resource { Name = "Associação de Moradores", Type = "Comunidade", Address = "Rua dos Vizinhos, 321", Icon = "🏘️", Distance = 1.8, Hours = "14:00-20:00", IsOpen = true });
        AllResources.Add(new Resource { Name = "CAPS II", Type = "Saúde", Address = "Rua Mental, 55", Icon = "🏥", Distance = 2.5, Hours = "24h", IsOpen = true });
        AllResources.Add(new Resource { Name = "Escola Municipal", Type = "Educação", Address = "Rua da Escola, 88", Icon = "📚", Distance = 0.8, Hours = "07:00-18:00", IsOpen = false });
    }

    [RelayCommand]
    public void Filter(string filterType)
    {
        SelectedFilter = filterType;
        ApplyFilter();
    }

    [RelayCommand]
    public async Task SearchLocation()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery)) return;

        IsLoading = true;
        try
        {
            var (state, city, success) = await _brasilApi.GetLocationByCEP(SearchQuery);
            if (success)
            {
                CurrentLocation = $"{city}, {state}";
                // In a real app, we would fetch resources for this location
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnSearchQueryChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredResources.Clear();
        var query = AllResources.AsEnumerable();

        if (SelectedFilter != "Todas")
        {
            // Simple mapping for demo
            if (SelectedFilter.Contains("Saúde")) query = query.Where(r => r.Type == "Saúde");
            else if (SelectedFilter.Contains("Educação")) query = query.Where(r => r.Type == "Educação");
            else if (SelectedFilter.Contains("Programas")) query = query.Where(r => r.Type == "Programas Sociais");
            else if (SelectedFilter.Contains("Economia")) query = query.Where(r => r.Type == "Economia Circular");
        }

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            // If search query looks like a CEP, we ignore it for text filtering (handled by SearchLocation)
            // But if it's text, we filter by name
            if (!char.IsDigit(SearchQuery.FirstOrDefault()))
            {
                query = query.Where(r => r.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
            }
        }

        foreach (var item in query)
        {
            FilteredResources.Add(item);
        }
    }
}
