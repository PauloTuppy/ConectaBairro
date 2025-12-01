using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de integração com Firebase Cloud Messaging
/// Gerencia push notifications e device tokens
/// </summary>
public class FirebaseService
{
    private static FirebaseService? _instance;
    public static FirebaseService Instance => _instance ??= new FirebaseService();

    public string? DeviceToken { get; private set; }
    public bool IsInitialized { get; private set; }
    
    public event EventHandler<PushNotification>? NotificationReceived;
    public event EventHandler<string>? TokenRefreshed;

    private readonly List<string> _subscribedTopics = new();

    private FirebaseService() { }

    /// <summary>
    /// Inicializa o Firebase Cloud Messaging
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            // Em produção, aqui seria a inicialização real do Firebase
            // Firebase.Messaging.FirebaseMessaging.Instance.GetToken()
            
            // Simulação de token para desenvolvimento
            DeviceToken = await GenerateDeviceTokenAsync();
            IsInitialized = true;
            
            System.Diagnostics.Debug.WriteLine($"Firebase inicializado. Token: {DeviceToken}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao inicializar Firebase: {ex.Message}");
            IsInitialized = false;
        }
    }

    private async Task<string> GenerateDeviceTokenAsync()
    {
        await Task.Delay(100); // Simula chamada assíncrona
        return $"fcm_token_{Guid.NewGuid():N}";
    }

    /// <summary>
    /// Inscreve o dispositivo em um tópico para receber notificações
    /// </summary>
    public async Task SubscribeToTopicAsync(string topic)
    {
        try
        {
            if (!IsInitialized) await InitializeAsync();
            
            if (!_subscribedTopics.Contains(topic))
            {
                _subscribedTopics.Add(topic);
                System.Diagnostics.Debug.WriteLine($"Inscrito no tópico: {topic}");
            }
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao inscrever no tópico {topic}: {ex.Message}");
        }
    }

    /// <summary>
    /// Remove inscrição de um tópico
    /// </summary>
    public async Task UnsubscribeFromTopicAsync(string topic)
    {
        try
        {
            _subscribedTopics.Remove(topic);
            System.Diagnostics.Debug.WriteLine($"Desinscrito do tópico: {topic}");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao desinscrever do tópico {topic}: {ex.Message}");
        }
    }

    /// <summary>
    /// Processa notificação recebida
    /// </summary>
    public void HandleNotification(string title, string body, Dictionary<string, string>? data = null)
    {
        var notification = new PushNotification
        {
            Title = title,
            Body = body,
            Data = data ?? new Dictionary<string, string>(),
            ReceivedAt = DateTime.Now
        };

        NotificationReceived?.Invoke(this, notification);
        
        // Vibrar dispositivo (Windows não suporta vibração diretamente)
        try
        {
            System.Diagnostics.Debug.WriteLine("Notificação recebida - vibração simulada");
        }
        catch { /* Ignorar se vibração não disponível */ }
    }

    /// <summary>
    /// Envia notificação local (para testes)
    /// </summary>
    public async Task SendLocalNotificationAsync(string title, string body)
    {
        HandleNotification(title, body);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Obtém lista de tópicos inscritos
    /// </summary>
    public IReadOnlyList<string> GetSubscribedTopics() => _subscribedTopics.AsReadOnly();
}

public class PushNotification
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public Dictionary<string, string> Data { get; set; } = new();
    public DateTime ReceivedAt { get; set; }
}
