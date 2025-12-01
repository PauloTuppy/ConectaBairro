using ConectaBairro.Models;
using ConectaBairro.Models.Entities;
using System.Collections.Immutable;

namespace ConectaBairro.Services;

public class CourseRepository : Repository<CourseEntity>, ICourseRepository
{
    public async Task<ImmutableList<Course>> SearchAsync(string? searchTerm)
    {
        var allEntities = await base.GetAllAsync();
        var allCourses = allEntities.Select(e => e.ToCourse()).ToList();
        
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return allCourses.ToImmutableList();
        }

        return allCourses
            .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       c.Provider.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToImmutableList();
    }

    public async Task<ImmutableList<Course>> GetByLocationAsync(string location)
    {
        var allEntities = await base.GetAllAsync();
        var allCourses = allEntities.Select(e => e.ToCourse()).ToList();
        return allCourses
            .Where(c => c.Location.Contains(location, StringComparison.OrdinalIgnoreCase))
            .ToImmutableList();
    }

    public async Task<ImmutableList<Course>> GetRecommendedAsync(UserProfile userProfile)
    {
        var allEntities = await base.GetAllAsync();
        var allCourses = allEntities.Select(e => e.ToCourse()).ToList();
        
        var recommendations = allCourses
            .Select(course => new
            {
                Course = course,
                Score = CalculateMatchScore(userProfile, course)
            })
            .Where(x => x.Score >= 50)
            .OrderByDescending(x => x.Score)
            .Take(10)
            .Select(x => x.Course with { MatchScore = x.Score })
            .ToImmutableList();

        return recommendations;
    }

    public new async Task<Course?> GetByIdAsync(Guid id)
    {
        var entity = await base.GetByIdAsync(id);
        return entity?.ToCourse();
    }

    public new async Task<List<Course>> GetAllAsync()
    {
        var entities = await base.GetAllAsync();
        return entities.Select(e => e.ToCourse()).ToList();
    }

    public async Task<int> InsertAsync(Course course)
    {
        var entity = CourseEntity.FromCourse(course);
        return await base.InsertAsync(entity);
    }

    public async Task<int> UpdateAsync(Course course)
    {
        var entity = CourseEntity.FromCourse(course);
        return await base.UpdateAsync(entity);
    }

    public async Task<int> DeleteAsync(Course course)
    {
        var entity = CourseEntity.FromCourse(course);
        return await base.DeleteAsync(entity);
    }

    private double CalculateMatchScore(UserProfile profile, Course course)
    {
        double score = 0;

        // Critério 1: Localização (30%)
        if (profile.Location.Contains(course.Location, StringComparison.OrdinalIgnoreCase))
        {
            score += 30;
        }
        else
        {
            score += 10;
        }

        // Critério 2: Escolaridade (25%)
        if (profile.EducationLevel >= course.MinEducationRequired)
        {
            score += 25;
        }

        // Critério 3: Interesses Alinhados (25%)
        int matchedInterests = profile.Interests
            .Count(interest => course.Areas.Contains(interest));
        
        if (matchedInterests > 0)
        {
            score += Math.Min(25, matchedInterests * 8);
        }

        // Critério 4: Idade (20%)
        int ageDifference = Math.Abs(profile.Age - (int)course.AverageStudentAge);
        if (ageDifference <= 10)
        {
            score += 20;
        }
        else if (ageDifference <= 20)
        {
            score += 10;
        }

        // Bônus: Programa elegível (10%)
        if (profile.EligiblePrograms.Contains(course.Program))
        {
            score += 10;
        }

        return Math.Min(100, score);
    }
}

