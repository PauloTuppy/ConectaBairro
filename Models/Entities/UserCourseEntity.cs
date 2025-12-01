using SQLite;
using System;

namespace ConectaBairro.Models.Entities;

public class UserCourseEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public Guid UserId { get; set; }

    [Indexed]
    public Guid CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    public DateTime? CompletionDate { get; set; } // Nullable, as course might not be completed
    public int ProgressPercentage { get; set; } = 0; // 0-100
    public bool IsCompleted => ProgressPercentage >= 100;
}
