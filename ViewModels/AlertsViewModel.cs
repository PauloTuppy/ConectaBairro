using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ConectaBairro.Models;

namespace ConectaBairro.ViewModels;

public partial class AlertsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Alert> alerts = new();

    public AlertsViewModel()
    {
        LoadMockAlerts();
    }

    private void LoadMockAlerts()
    {
        Alerts.Add(new Alert
        {
            Title = "Chuva forte prevista",
            Message = "Rua das Flores - Atenção para alagamentos",
            Type = AlertType.Warning,
            Location = "Rua das Flores"
        });

        Alerts.Add(new Alert
        {
            Title = "Falta de água",
            Message = "Vila Nova das 14h às 20h",
            Type = AlertType.Emergency,
            Location = "Vila Nova"
        });

        Alerts.Add(new Alert
        {
            Title = "Caronas solidárias",
            Message = "Motorista oferecendo caronas para região",
            Type = AlertType.Opportunity,
            Location = "Centro"
        });

        Alerts.Add(new Alert
        {
            Title = "Reunião comunitária",
            Message = "Sábado às 10h na praça central",
            Type = AlertType.Info,
            Location = "Praça Central"
        });

        Alerts.Add(new Alert
        {
            Title = "Novo programa social",
            Message = "Inscrições abertas para curso gratuito",
            Type = AlertType.Opportunity,
            Location = "Centro Comunitário"
        });
    }
}
