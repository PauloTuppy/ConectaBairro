using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ConectaBairro.ViewModels;

public class OnboardingItem
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
}

public partial class OnboardingViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanGoNext))]
    [NotifyPropertyChangedFor(nameof(CanGoPrevious))]
    [NotifyPropertyChangedFor(nameof(IsLastSlide))]
    private int currentSlideIndex = 0;

    public ObservableCollection<OnboardingItem> Items { get; } = new();

    public bool CanGoNext => CurrentSlideIndex < Items.Count - 1;
    public bool CanGoPrevious => CurrentSlideIndex > 0;
        public bool IsLastSlide => CurrentSlideIndex == Items.Count - 1;
    
        public Action? StartNavigationRequested { get; set; }
    
        public OnboardingViewModel()
        {
            Items.Add(new OnboardingItem
            {
                Icon = "🌍",
                Title = "Transforme Seu Bairro",
                Description = "Encontre oportunidades de qualificação profissional, recursos públicos e conexões comunitárias"
            });
            Items.Add(new OnboardingItem
            {
                Icon = "💼",
                Title = "Acesso Fácil",
                Description = "Autonomia e Renda Petrobras, PRONATEC e muito mais. Com bolsa-auxílio de até R$ 858/mês"
            });
            Items.Add(new OnboardingItem
            {
                Icon = "🔄",
                Title = "Comunidade",
                Description = "Compartilhe habilidades, recursos e transforme sua comunidade junto"
            });
        }
        
        // ... (other methods)
    
        [RelayCommand]
        public void Start()
        {
            StartNavigationRequested?.Invoke();
        }
    public void UpdateSlideIndex(int index)
    {
        CurrentSlideIndex = index;
    }
}
