using ConectaBairro.Models;

namespace ConectaBairro.Services;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetCurrentUserAsync();
    Task<int> SaveCurrentUserAsync(UserProfile user);
    Task<List<Course>> GetEnrolledCoursesAsync(Guid userId);
    Task<List<Course>> GetCompletedCoursesAsync(Guid userId);
    Task<UserProfile?> GetByIdAsync(Guid id);
    Task<List<UserProfile>> GetAllAsync();
}

