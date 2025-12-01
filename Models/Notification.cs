namespace ConectaBairro.Models;

public enum NotificationType
{
    Course,
    Badge,
    Alert,
    Event,
    Reminder,
    XP
}

public record Notification
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = "";
    public string Message { get; init; } = "";
    public NotificationType Type { get; init; }
    public string Icon { get; init; } = "🔔";
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public bool IsRead { get; init; }
    public string? ActionUrl { get; init; }
    
    public string TypeColor => Type switch
    {
        NotificationType.Course => "#3B82F6",
        NotificationType.Badge => "#22C55E",
        NotificationType.Alert => "#EF4444",
        NotificationType.Event => "#F59E0B",
        NotificationType.Reminder => "#8B5CF6",
        NotificationType.XP => "#10B981",
        _ => "#6B7280"
    };

    public string TimeAgo
    {
        get
        {
            var diff = DateTime.Now - CreatedAt;
            if (diff.TotalMinutes < 60) return $"Há {(int)diff.TotalMinutes} minutos";
            if (diff.TotalHours < 24) return $"Há {(int)diff.TotalHours} horas";
            if (diff.TotalDays < 7) return $"Há {(int)diff.TotalDays} dias";
            return CreatedAt.ToString("dd/MM/yyyy");
        }
    }
}
