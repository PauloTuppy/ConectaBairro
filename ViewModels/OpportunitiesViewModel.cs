using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConectaBairro.Services;
using System.Collections.ObjectModel;

namespace ConectaBairro.ViewModels;

public partial class OpportunitiesViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OpportunityProgram> opportunities = new();

    [ObservableProperty]
    private ObservableCollection<OpportunityProgram> filteredOpportunities = new();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string selectedProgram = "Todos";

    [ObservableProperty]
    private string searchQuery = "";

    public string[] Programs { get; } = { "Todos", "Autonomia e Renda", "PRONATEC", "SENAC", "SESC" };

    public OpportunitiesViewModel()
    {
        LoadOpportunitiesAsync();
    }

    private async void LoadOpportunitiesAsync()
    {
        IsLoading = true;
        try
        {
            var data = await OpportunitiesService.GetAllOpportunitiesAsync("SP");
            Opportunities = new ObservableCollection<OpportunityProgram>(data);
            FilterOpportunities();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void SelectProgram(string program)
    {
        SelectedProgram = program;
        FilterOpportunities();
    }

    [RelayCommand]
    private void Search()
    {
        FilterOpportunities();
    }

    private void FilterOpportunities()
    {
        var filtered = Opportunities.AsEnumerable();

        if (SelectedProgram != "Todos")
        {
            filtered = filtered.Where(o => o.Program == SelectedProgram);
        }

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            var query = SearchQuery.ToLower();
            filtered = filtered.Where(o =>
                o.Name.ToLower().Contains(query) ||
                o.Description.ToLower().Contains(query) ||
                o.Category.ToLower().Contains(query));
        }

        FilteredOpportunities = new ObservableCollection<OpportunityProgram>(filtered);
    }

    [RelayCommand]
    private async Task Enroll(OpportunityProgram opportunity)
    {
        // Simular inscrição e ganho de XP
        await Task.Delay(500);
        
        // Adicionar notificação
        NotificationService.Instance.AddNotification(new Models.Notification
        {
            Title = "Inscrição realizada!",
            Message = $"Você se inscreveu em {opportunity.Name}. +50 XP!",
            Type = Models.NotificationType.Course,
            Icon = "✅"
        });
    }
}
