using ConectaBairro.Models;
using System.Collections.ObjectModel;

namespace ConectaBairro.Services;

public class NotificationService
{
    private static NotificationService? _instance;
    public static NotificationService Instance => _instance ??= new NotificationService();

    public ObservableCollection<Notification> Notifications { get; } = new();
    
    public int UnreadCount => Notifications.Count(n => !n.IsRead);

    public event Action? OnNotificationAdded;
    public event Action? OnNotificationsChanged;

    private NotificationService()
    {
        LoadMockNotifications();
    }

    private void LoadMockNotifications()
    {
        Notifications.Add(new Notification
        {
            Title = "Nova vaga disponível!",
            Message = "Curso de Operador de Computador abriu 20 novas vagas",
            Type = NotificationType.Course,
            Icon = "📚",
            CreatedAt = DateTime.Now.AddMinutes(-15)
        });

        Notifications.Add(new Notification
        {
            Title = "Parabéns! +50 XP",
            Message = "Você completou o módulo 'Introdução à Informática'",
            Type = NotificationType.XP,
            Icon = "🏆",
            CreatedAt = DateTime.Now.AddHours(-2)
        });

        Notifications.Add(new Notification
        {
            Title = "Alerta Comunitário",
            Message = "Chuva forte prevista para sua região nas próximas horas",
            Type = NotificationType.Alert,
            Icon = "⚠️",
            CreatedAt = DateTime.Now.AddHours(-3)
        });

        Notifications.Add(new Notification
        {
            Title = "Lembrete de inscrição",
            Message = "Faltam 5 dias para o fim das inscrições do PRONATEC",
            Type = NotificationType.Reminder,
            Icon = "📅",
            CreatedAt = DateTime.Now.AddDays(-1),
            IsRead = true
        });

        Notifications.Add(new Notification
        {
            Title = "Evento na comunidade",
            Message = "Mutirão de limpeza confirmado para sábado",
            Type = NotificationType.Event,
            Icon = "🎉",
            CreatedAt = DateTime.Now.AddDays(-1),
            IsRead = true
        });

        Notifications.Add(new Notification
        {
            Title = "Autonomia e Renda",
            Message = "Novas vagas abertas com bolsa de R$ 858/mês",
            Type = NotificationType.Course,
            Icon = "💼",
            CreatedAt = DateTime.Now.AddDays(-3),
            IsRead = true
        });

        Notifications.Add(new Notification
        {
            Title = "Badge desbloqueado!",
            Message = "Você conquistou 'Mestre do Tempo' por entregar tudo no prazo",
            Type = NotificationType.Badge,
            Icon = "🏅",
            CreatedAt = DateTime.Now.AddDays(-4),
            IsRead = true
        });
    }

    public void AddNotification(Notification notification)
    {
        Notifications.Insert(0, notification);
        OnNotificationAdded?.Invoke();
        OnNotificationsChanged?.Invoke();
    }

    public void MarkAsRead(Guid notificationId)
    {
        var notification = Notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            var index = Notifications.IndexOf(notification);
            Notifications[index] = notification with { IsRead = true };
            OnNotificationsChanged?.Invoke();
        }
    }

    public void MarkAllAsRead()
    {
        for (int i = 0; i < Notifications.Count; i++)
        {
            if (!Notifications[i].IsRead)
            {
                Notifications[i] = Notifications[i] with { IsRead = true };
            }
        }
        OnNotificationsChanged?.Invoke();
    }

    public void ClearAll()
    {
        Notifications.Clear();
        OnNotificationsChanged?.Invoke();
    }

    public void RemoveNotification(Guid notificationId)
    {
        var notification = Notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            Notifications.Remove(notification);
            OnNotificationsChanged?.Invoke();
        }
    }
}
