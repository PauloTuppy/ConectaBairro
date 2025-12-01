using ConectaBairro.Models.Entities;

namespace ConectaBairro.Models;

public record UserProfile
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = "";
    public int Age { get; init; } = 25;
    public EducationLevel EducationLevel { get; init; }
    public string Location { get; init; } = ""; // CEP ou bairro
    public string[] Interests { get; init; } = Array.Empty<string>();
    public ProgramType[] EligiblePrograms { get; init; } = Array.Empty<ProgramType>();
    public int XP { get; init; } = 0;
    public int CurrentLevel { get; init; } = 1;
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public string UserId { get; init; } = ""; // Para autenticação futura

    // Cursos que se inscreveu
    public List<Guid> EnrolledCourseIds { get; init; } = new();
    // Cursos que completou
    public List<Guid> CompletedCourseIds { get; init; } = new();

    // Calculated properties for UI
    public int NextLevelXP => CurrentLevel * 500;
    public string LevelDescription
    {
        get
        {
            return $"Nível {CurrentLevel}: {GetLevelName(CurrentLevel)}";
        }
    }

    private string GetLevelName(int level)
    {
        if (level < 5) return "Iniciante";
        if (level < 10) return "Aprendiz";
        if (level < 15) return "Conhecedor";
        if (level < 20) return "Mentor";
        return "Especialista";
    }

    public UserProfileEntity ToEntity() => UserProfileEntity.FromUserProfile(this);
}
