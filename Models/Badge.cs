using ConectaBairro.Models.Entities;

namespace ConectaBairro.Models;

public record Badge
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public string IconEmoji { get; init; } = "🏆";
    public DateTime? UnlockedDate { get; init; } = null;

    public bool IsUnlocked => UnlockedDate.HasValue;

    public BadgeEntity ToEntity() => BadgeEntity.FromBadge(this);
}
