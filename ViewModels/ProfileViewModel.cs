using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConectaBairro.Models;
using ConectaBairro.MockData;
using System.Collections.ObjectModel;

namespace ConectaBairro.ViewModels;

public class UserStatistics
{
    public int EnrolledCourses { get; set; }
    public int CompletedCourses { get; set; }
    public decimal CommunitySavings { get; set; }
    public int NeighborsHelped { get; set; }
    public DateTime JoinDate { get; set; }
    public int TotalStudyHours { get; set; }
    public int SharesCount { get; set; }
    public int CommunitiesCreated { get; set; }
}

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    private UserProfile user = new();

    [ObservableProperty]
    private UserStatistics stats = new();

    public ObservableCollection<Badge> UnlockedBadges { get; } = new();
    public ObservableCollection<Badge> LockedBadges { get; } = new();

    private readonly Services.IUserProfileRepository _userRepository = new Services.UserProfileRepository();
    private readonly Services.ICourseRepository _courseRepository = new Services.CourseRepository();

    public ProfileViewModel()
    {
        LoadUserProfile();
    }

    private async void LoadUserProfile()
    {
        try
        {
            User = await _userRepository.GetCurrentUserAsync() ?? new UserProfile
            {
                Name = "Usuário",
                Age = 25,
                Location = "São Paulo, SP",
                XP = 0,
                CurrentLevel = 1
            };

            // Carregar estatísticas
            if (User != null)
            {
                // In a real application, if XP was just gained,
                // AnimationService.XPGainAsync(uiElement, xpAmount) could be triggered from the View
                // after observing User.XP change.
                var enrolledCourses = await _userRepository.GetEnrolledCoursesAsync(User.Id);
                var completedCourses = await _userRepository.GetCompletedCoursesAsync(User.Id);

                Stats = new UserStatistics
                {
                    EnrolledCourses = enrolledCourses.Count,
                    CompletedCourses = completedCourses.Count,
                    CommunitySavings = completedCourses.Sum(c => (decimal)c.Stipend),
                    NeighborsHelped = 0, // TODO: Implementar quando houver feature de comunidade
                    JoinDate = User.CreatedAt,
                    TotalStudyHours = completedCourses.Sum(c => c.WeeklyHours * 4), // Estimativa
                    SharesCount = 0, // TODO: Implementar
                    CommunitiesCreated = 0 // TODO: Implementar
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar perfil: {ex.Message}");
            // Fallback para dados mockados
            User = new UserProfile
            {
                Name = "Paulo Silva",
                Age = 28,
                Location = "São Paulo, SP",
                XP = 450,
                CurrentLevel = 5
            };
            Stats = new UserStatistics
            {
                EnrolledCourses = 3,
                CompletedCourses = 1,
                CommunitySavings = 1240.50m,
                NeighborsHelped = 12,
                JoinDate = new DateTime(2025, 1, 15),
                TotalStudyHours = 42,
                SharesCount = 8,
                CommunitiesCreated = 1
            };
        }
        
        LoadBadges();
    }

    private void LoadBadges()
    {
        try
        {
            // Por enquanto usa mock data, mas pode ser migrado para repositório depois
            var allBadges = MockBadges.GetMockBadges();
            
            foreach (var badge in allBadges)
            {
                if (badge.IsUnlocked)
                {
                    UnlockedBadges.Add(badge);
                }
                else
                {
                    LockedBadges.Add(badge);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar badges: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task EditProfile()
    {
        if (User != null)
        {
            await _userRepository.SaveCurrentUserAsync(User);
        }
    }

    [RelayCommand]
    public void ShareProgress()
    {
        // Logic to share progress
    }

    [RelayCommand]
    public void Logout()
    {
        // Logic to logout
    }
}
