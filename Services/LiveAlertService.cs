using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de alertas ao vivo
/// Simula alertas em tempo real do bairro
/// </summary>
public class LiveAlertService
{
    private static LiveAlertService? _instance;
    public static LiveAlertService Instance => _instance ??= new LiveAlertService();

    private readonly System.Timers.Timer _alertTimer;
    private readonly List<LiveAlert> _activeAlerts = new();
    private readonly Random _random = new();

    public event EventHandler<LiveAlert>? NewAlertReceived;
    public event EventHandler<LiveAlert>? AlertExpired;

    public IReadOnlyList<LiveAlert> ActiveAlerts => _activeAlerts.AsReadOnly();

    private LiveAlertService()
    {
        _alertTimer = new System.Timers.Timer(30000); // Verifica a cada 30 segundos
        _alertTimer.Elapsed += CheckForNewAlerts;
        
        // Inicializar com alguns alertas
        InitializeAlerts();
    }

    public void StartMonitoring()
    {
        _alertTimer.Start();
        System.Diagnostics.Debug.WriteLine("LiveAlertService: Monitoramento iniciado");
    }

    public void StopMonitoring()
    {
        _alertTimer.Stop();
    }

    private void InitializeAlerts()
    {
        _activeAlerts.AddRange(new[]
        {
            new LiveAlert
            {
                Id = Guid.NewGuid(),
                Title = "🚰 Falta d'água programada",
                Description = "Manutenção na rede - Rua das Flores, 14h-18h",
                Type = LiveAlertType.Infrastructure,
                Severity = AlertSeverity.Medium,
                Latitude = -23.5510,
                Longitude = -46.6340,
                CreatedAt = DateTime.Now.AddHours(-2),
                ExpiresAt = DateTime.Now.AddHours(4),
                IsPulsing = true
            },
            new LiveAlert
            {
                Id = Guid.NewGuid(),
                Title = "📢 Vagas abertas SENAI",
                Description = "50 vagas para curso de Programação - Inscrições até sexta!",
                Type = LiveAlertType.Opportunity,
                Severity = AlertSeverity.Low,
                Latitude = -23.5289,
                Longitude = -46.7195,
                CreatedAt = DateTime.Now.AddDays(-1),
                ExpiresAt = DateTime.Now.AddDays(3),
                IsPulsing = true
            },
            new LiveAlert
            {
                Id = Guid.NewGuid(),
                Title = "🏥 Campanha de vacinação",
                Description = "UBS Centro - Gripe e COVID, sem agendamento",
                Type = LiveAlertType.Health,
                Severity = AlertSeverity.High,
                Latitude = -23.5505,
                Longitude = -46.6333,
                CreatedAt = DateTime.Now.AddHours(-5),
                ExpiresAt = DateTime.Now.AddDays(7),
                IsPulsing = true
            }
        });
    }

    private void CheckForNewAlerts(object? sender, System.Timers.ElapsedEventArgs e)
    {
        // Remover alertas expirados
        var expired = _activeAlerts.FindAll(a => a.ExpiresAt < DateTime.Now);
        foreach (var alert in expired)
        {
            _activeAlerts.Remove(alert);
            AlertExpired?.Invoke(this, alert);
        }

        // Simular novo alerta aleatório (10% de chance)
        if (_random.Next(10) == 0)
        {
            var newAlert = GenerateRandomAlert();
            _activeAlerts.Add(newAlert);
            NewAlertReceived?.Invoke(this, newAlert);
        }
    }

    private LiveAlert GenerateRandomAlert()
    {
        var alerts = new[]
        {
            ("🚧 Obra na via", "Interdição parcial na Av. Principal", LiveAlertType.Infrastructure, AlertSeverity.Medium),
            ("📚 Feira de livros", "Praça Central - Livros a partir de R$5", LiveAlertType.Event, AlertSeverity.Low),
            ("💼 Mutirão de emprego", "CAT Centro - 200 vagas disponíveis", LiveAlertType.Opportunity, AlertSeverity.High),
            ("🌧️ Alerta de chuva", "Previsão de chuva forte às 18h", LiveAlertType.Weather, AlertSeverity.Medium),
            ("🎓 Inscrições abertas", "ETEC - Vestibulinho 2º semestre", LiveAlertType.Opportunity, AlertSeverity.High)
        };

        var selected = alerts[_random.Next(alerts.Length)];
        
        return new LiveAlert
        {
            Id = Guid.NewGuid(),
            Title = selected.Item1,
            Description = selected.Item2,
            Type = selected.Item3,
            Severity = selected.Item4,
            Latitude = -23.5505 + (_random.NextDouble() - 0.5) * 0.02,
            Longitude = -46.6333 + (_random.NextDouble() - 0.5) * 0.02,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddHours(_random.Next(2, 24)),
            IsPulsing = true
        };
    }

    public async Task<List<LiveAlert>> GetAlertsNearbyAsync(double lat, double lng, double radiusKm = 5)
    {
        var nearby = new List<LiveAlert>();
        foreach (var alert in _activeAlerts)
        {
            var distance = MapService.Instance.CalculateDistance(lat, lng, alert.Latitude, alert.Longitude);
            if (distance <= radiusKm)
            {
                alert.DistanceKm = distance;
                nearby.Add(alert);
            }
        }
        await Task.CompletedTask;
        return nearby;
    }
}

public class LiveAlert
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public LiveAlertType Type { get; set; }
    public AlertSeverity Severity { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsPulsing { get; set; }
    public double DistanceKm { get; set; }
    
    public string TimeAgo
    {
        get
        {
            var diff = DateTime.Now - CreatedAt;
            if (diff.TotalMinutes < 60) return $"há {(int)diff.TotalMinutes} min";
            if (diff.TotalHours < 24) return $"há {(int)diff.TotalHours}h";
            return $"há {(int)diff.TotalDays} dias";
        }
    }

    public string SeverityColor => Severity switch
    {
        AlertSeverity.High => "#EF4444",
        AlertSeverity.Medium => "#F59E0B",
        AlertSeverity.Low => "#10B981",
        _ => "#6B7280"
    };
}

public enum LiveAlertType { Infrastructure, Health, Opportunity, Event, Weather, Safety }
public enum AlertSeverity { Low, Medium, High }
