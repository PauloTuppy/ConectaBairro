using ConectaBairro.Models;
using ConectaBairro.Models.Entities;
using SQLite;

namespace ConectaBairro.Services;

public class UserProfileRepository : Repository<UserProfileEntity>, IUserProfileRepository
{
    public async Task<UserProfile?> GetCurrentUserAsync()
    {
        // Por enquanto, retorna o primeiro usuário ou cria um padrão
        // Em produção, isso viria de autenticação
        var entities = await base.GetAllAsync();
        if (entities.Any())
        {
            return entities.First().ToUserProfile();
        }

        // Se não houver usuário, cria um padrão
        var defaultUser = new UserProfile
        {
            Name = "Usuário",
            Age = 25,
            EducationLevel = EducationLevel.Medio,
            Location = "São Paulo, SP",
            XP = 0,
            CurrentLevel = 1
        };

        var entity = UserProfileEntity.FromUserProfile(defaultUser);
        await InsertAsync(entity);
        return defaultUser;
    }

    public async Task<int> SaveCurrentUserAsync(UserProfile user)
    {
        var entity = UserProfileEntity.FromUserProfile(user);
        var existing = await GetByIdAsync(user.Id);
        if (existing != null)
        {
            return await UpdateAsync(entity);
        }
        return await InsertAsync(entity);
    }

    public async Task<List<Course>> GetEnrolledCoursesAsync(Guid userId)
    {
        var userEntity = await base.GetByIdAsync(userId);
        if (userEntity == null)
        {
            return new List<Course>();
        }

        var user = userEntity.ToUserProfile();
        if (!user.EnrolledCourseIds.Any())
        {
            return new List<Course>();
        }

        var courseRepository = new CourseRepository();
        var courses = new List<Course>();
        
        foreach (var courseId in user.EnrolledCourseIds)
        {
            var course = await courseRepository.GetByIdAsync(courseId);
            if (course != null)
            {
                courses.Add(course);
            }
        }

        return courses;
    }

    public async Task<List<Course>> GetCompletedCoursesAsync(Guid userId)
    {
        var userEntity = await base.GetByIdAsync(userId);
        if (userEntity == null)
        {
            return new List<Course>();
        }

        var user = userEntity.ToUserProfile();
        if (!user.CompletedCourseIds.Any())
        {
            return new List<Course>();
        }

        var courseRepository = new CourseRepository();
        var courses = new List<Course>();
        
        foreach (var courseId in user.CompletedCourseIds)
        {
            var course = await courseRepository.GetByIdAsync(courseId);
            if (course != null)
            {
                courses.Add(course);
            }
        }

        return courses;
    }

    public new async Task<UserProfile?> GetByIdAsync(Guid id)
    {
        var entity = await base.GetByIdAsync(id);
        return entity?.ToUserProfile();
    }

    public new async Task<List<UserProfile>> GetAllAsync()
    {
        var entities = await base.GetAllAsync();
        return entities.Select(e => e.ToUserProfile()).ToList();
    }
}

