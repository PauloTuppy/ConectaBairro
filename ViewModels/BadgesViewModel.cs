using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input; // Added this line
using System.Collections.ObjectModel;
using ConectaBairro.Models;
using ConectaBairro.MockData;
using System.Linq; // For .Any()
using System; // For DateTime

namespace ConectaBairro.ViewModels;

public partial class BadgesViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Badge> unlockedBadges = new();

    [ObservableProperty]
    private ObservableCollection<Badge> lockedBadges = new();

    [ObservableProperty]
    private ObservableCollection<JourneyPhase> journeyPhases = new(); // New property

    [ObservableProperty]
    private int currentXP = 0;

    [ObservableProperty]
    private int nextLevelXP = 500; // Example value

    [ObservableProperty]
    private string levelDescription = "Nível 1: Iniciante";

    public BadgesViewModel()
    {
        LoadBadges();
        LoadUserProfileData(); // Simulate loading user XP/level
        LoadJourneyPhases(); // New method call
    }

    [RelayCommand]
    private void UnlockNextBadge()
    {
        if (LockedBadges.Any())
        {
            var badgeToUnlock = LockedBadges.First();
            LockedBadges.Remove(badgeToUnlock);

            // Create a new instance of the record with updated properties
            var unlockedBadge = badgeToUnlock with { UnlockedDate = DateTime.Now };
            UnlockedBadges.Add(unlockedBadge);

            // In a real application, the AnimationService.BadgeUnlockAsync(uiElement)
            // would be triggered from the View (e.g., code-behind or a behavior)
            // after observing this badge being added to UnlockedBadges.
            System.Diagnostics.Debug.WriteLine($"Badge '{unlockedBadge.Name}' unlocked!");
        }
    }

    private void LoadBadges()
    {
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

    private void LoadUserProfileData()
    {
        // Simulate loading user data for XP and level
        // In a real app, this would come from a service/repository
        CurrentXP = 320; // Example
        NextLevelXP = 500; // Example
        LevelDescription = "Nível 4: Explorador"; // Example
    }

    private void LoadJourneyPhases()
    {
        JourneyPhases.Add(new JourneyPhase
        {
            Name = "Fase 1: Descoberta",
            Description = "Explore 12 recursos locais.",
            CurrentProgress = 80, // 80% complete
            TargetProgress = 100,
            XPReward = 50,
            IconEmoji = "🎯"
        });
        JourneyPhases.Add(new JourneyPhase
        {
            Name = "Fase 2: Qualificação",
            Description = "Inscreva-se em um curso PRONATEC próximo.",
            CurrentProgress = 0,
            TargetProgress = 100,
            XPReward = 100,
            IconEmoji = "🎓"
        });
        JourneyPhases.Add(new JourneyPhase
        {
            Name = "Fase 3: Engajamento",
            Description = "Participe de 3 eventos comunitários.",
            CurrentProgress = 0,
            TargetProgress = 100,
            XPReward = 75,
            IconEmoji = "🤝"
        });
    }
}
