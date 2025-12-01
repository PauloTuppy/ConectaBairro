using ConectaBairro.Models.Entities;

namespace ConectaBairro.Models;

public record Course
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public string Provider { get; init; } = ""; // SENAI, SENAC, IFPE, IFRS
    public ProgramType Program { get; init; }
    public string Duration { get; init; } = "6 meses"; // Duração do curso
    public int WeeklyHours { get; init; } = 20;
    public decimal Stipend { get; init; } = 0; // Bolsa mensal em R$
    public string Location { get; init; } = ""; // Bairro/Cidade
    public int AvailableVacancies { get; init; } = 0;
    public EducationLevel MinEducationRequired { get; init; }
    public string[] Areas { get; init; } = Array.Empty<string>(); // [Técnico, Industrial, Saúde]
    public DateTime EnrollmentStartDate { get; init; }
    public DateTime EnrollmentDeadline { get; init; }
    public string ThumbnailUrl { get; init; } = "";
    public double AverageStudentAge { get; init; } = 30;

    // Para recomendação
    public double MatchScore { get; init; } = 0;

    public CourseEntity ToEntity() => CourseEntity.FromCourse(this);
}
