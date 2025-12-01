using ConectaBairro.Models;
using System.Collections.Immutable;

namespace ConectaBairro.Services;

public interface ICourseRepository
{
    Task<ImmutableList<Course>> SearchAsync(string? searchTerm);
    Task<ImmutableList<Course>> GetByLocationAsync(string location);
    Task<ImmutableList<Course>> GetRecommendedAsync(UserProfile userProfile);
    Task<Course?> GetByIdAsync(Guid id);
    Task<List<Course>> GetAllAsync();
    Task<int> InsertAsync(Course course);
    Task<int> UpdateAsync(Course course);
    Task<int> DeleteAsync(Course course);
}

