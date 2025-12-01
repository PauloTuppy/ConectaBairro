using SQLite;
using ConectaBairro.Models;

namespace ConectaBairro.Models.Entities;

[Table("Course")]
public class CourseEntity
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Provider { get; set; } = "";
    public ProgramType Program { get; set; }
    public string Duration { get; set; } = "6 meses";
    public int WeeklyHours { get; set; } = 20;
    public decimal Stipend { get; set; } = 0;
    public string Location { get; set; } = "";
    public int AvailableVacancies { get; set; } = 0;
    public EducationLevel MinEducationRequired { get; set; }
    public string AreasJson { get; set; } = "[]";
    public DateTime EnrollmentStartDate { get; set; }
    public DateTime EnrollmentDeadline { get; set; }
    public string ThumbnailUrl { get; set; } = "";
    public double AverageStudentAge { get; set; } = 30;
    public double MatchScore { get; set; } = 0;

    public Course ToCourse()
    {
        return new Course
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Provider = Provider,
            Program = Program,
            Duration = Duration,
            WeeklyHours = WeeklyHours,
            Stipend = Stipend,
            Location = Location,
            AvailableVacancies = AvailableVacancies,
            MinEducationRequired = MinEducationRequired,
            Areas = System.Text.Json.JsonSerializer.Deserialize<string[]>(AreasJson) ?? Array.Empty<string>(),
            EnrollmentStartDate = EnrollmentStartDate,
            EnrollmentDeadline = EnrollmentDeadline,
            ThumbnailUrl = ThumbnailUrl,
            AverageStudentAge = AverageStudentAge,
            MatchScore = MatchScore
        };
    }

    public static CourseEntity FromCourse(Course course)
    {
        return new CourseEntity
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            Provider = course.Provider,
            Program = course.Program,
            Duration = course.Duration,
            WeeklyHours = course.WeeklyHours,
            Stipend = course.Stipend,
            Location = course.Location,
            AvailableVacancies = course.AvailableVacancies,
            MinEducationRequired = course.MinEducationRequired,
            AreasJson = System.Text.Json.JsonSerializer.Serialize(course.Areas),
            EnrollmentStartDate = course.EnrollmentStartDate,
            EnrollmentDeadline = course.EnrollmentDeadline,
            ThumbnailUrl = course.ThumbnailUrl,
            AverageStudentAge = course.AverageStudentAge,
            MatchScore = course.MatchScore
        };
    }
}

