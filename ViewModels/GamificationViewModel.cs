using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConectaBairro.Models;
using ConectaBairro.MockData;

namespace ConectaBairro.ViewModels;

public partial class GamificationViewModel : ObservableObject
{
    [ObservableProperty]
    private UserProfile userProfile;

    [ObservableProperty]
    private ImmutableList<Badge> badges = ImmutableList<Badge>.Empty;

    public GamificationViewModel()
    {
        // Initialize with mock data
        UserProfile = new UserProfile
        {
            Name = "Usuário Exemplo",
            XP = 1250,
            CurrentLevel = 5
        };
        
        LoadBadges();
    }

    private void LoadBadges()
    {
        Badges = MockBadges.GetMockBadges().ToImmutableList();
    }
}
