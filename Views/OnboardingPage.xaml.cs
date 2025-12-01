using ConectaBairro.ViewModels;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class OnboardingPage : Page
{
    private OnboardingViewModel ViewModel => (OnboardingViewModel)this.DataContext;

    public OnboardingPage()
    {
        this.InitializeComponent();
        var viewModel = new OnboardingViewModel();
        // Navega para LoginPage ao invés de DashboardPage
        viewModel.StartNavigationRequested = () => NavigationService.NavigateTo<LoginPage>();
        this.DataContext = viewModel;
    }
}
