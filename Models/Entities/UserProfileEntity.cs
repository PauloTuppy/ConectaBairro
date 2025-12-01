using SQLite;
using ConectaBairro.Models;

namespace ConectaBairro.Models.Entities;

[Table("UserProfile")]
public class UserProfileEntity
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; } = "";
    public int Age { get; set; } = 25;
    public EducationLevel EducationLevel { get; set; }
    public string Location { get; set; } = "";
    public string InterestsJson { get; set; } = "[]"; // Serializado como JSON
    public string EligibleProgramsJson { get; set; } = "[]"; // Serializado como JSON
    public int XP { get; set; } = 0;
    public int CurrentLevel { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string UserId { get; set; } = "";
    public string EnrolledCourseIdsJson { get; set; } = "[]";
    public string CompletedCourseIdsJson { get; set; } = "[]";

    public UserProfile ToUserProfile()
    {
        return new UserProfile
        {
            Id = Id,
            Name = Name,
            Age = Age,
            EducationLevel = EducationLevel,
            Location = Location,
            Interests = System.Text.Json.JsonSerializer.Deserialize<string[]>(InterestsJson) ?? Array.Empty<string>(),
            EligiblePrograms = System.Text.Json.JsonSerializer.Deserialize<ProgramType[]>(EligibleProgramsJson) ?? Array.Empty<ProgramType>(),
            XP = XP,
            CurrentLevel = CurrentLevel,
            CreatedAt = CreatedAt,
            UserId = UserId,
            EnrolledCourseIds = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(EnrolledCourseIdsJson) ?? new List<Guid>(),
            CompletedCourseIds = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(CompletedCourseIdsJson) ?? new List<Guid>()
        };
    }

    public static UserProfileEntity FromUserProfile(UserProfile profile)
    {
        return new UserProfileEntity
        {
            Id = profile.Id,
            Name = profile.Name,
            Age = profile.Age,
            EducationLevel = profile.EducationLevel,
            Location = profile.Location,
            InterestsJson = System.Text.Json.JsonSerializer.Serialize(profile.Interests),
            EligibleProgramsJson = System.Text.Json.JsonSerializer.Serialize(profile.EligiblePrograms),
            XP = profile.XP,
            CurrentLevel = profile.CurrentLevel,
            CreatedAt = profile.CreatedAt,
            UserId = profile.UserId,
            EnrolledCourseIdsJson = System.Text.Json.JsonSerializer.Serialize(profile.EnrolledCourseIds),
            CompletedCourseIdsJson = System.Text.Json.JsonSerializer.Serialize(profile.CompletedCourseIds)
        };
    }
}

