using SQLite;

namespace ConectaBairro.Models.Entities;

[Table("Badge")]
public class BadgeEntity
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string IconEmoji { get; set; } = "🏆";
    public DateTime? UnlockedDate { get; set; } = null;

    public Badge ToBadge()
    {
        return new Badge
        {
            Id = Id,
            Name = Name,
            Description = Description,
            IconEmoji = IconEmoji,
            UnlockedDate = UnlockedDate
        };
    }

    public static BadgeEntity FromBadge(Badge badge)
    {
        return new BadgeEntity
        {
            Id = badge.Id,
            Name = badge.Name,
            Description = badge.Description,
            IconEmoji = badge.IconEmoji,
            UnlockedDate = badge.UnlockedDate
        };
    }
}


