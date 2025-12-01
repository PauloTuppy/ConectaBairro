using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ConectaBairro.Services;

namespace ConectaBairro.Views;

public sealed partial class LoginPage : Page
{
    public LoginPage()
    {
        InitializeComponent();
        CheckExistingSession();
    }

    private async void CheckExistingSession()
    {
        ShowLoading("Verificando sessão...");
        
        var restored = await OAuthService.Instance.RestoreSessionAsync();
        if (restored)
        {
            NavigationService.NavigateTo<DashboardPage>();
            return;
        }
        
        HideLoading();
    }

    private async void GoogleLogin_Click(object sender, RoutedEventArgs e)
    {
        ShowLoading("Conectando com Google...");
        StatusText.Text = "";

        var result = await OAuthService.Instance.LoginWithGoogleAsync();
        
        if (result.Success)
        {
            await AnimationService.FadeIn(this);
            NavigationService.NavigateTo<DashboardPage>();
        }
        else
        {
            StatusText.Text = result.Error;
            HideLoading();
        }
    }

    private async void GitHubLogin_Click(object sender, RoutedEventArgs e)
    {
        ShowLoading("Conectando com GitHub...");
        StatusText.Text = "";

        var result = await OAuthService.Instance.LoginWithGitHubAsync();
        
        if (result.Success)
        {
            await AnimationService.FadeIn(this);
            NavigationService.NavigateTo<DashboardPage>();
        }
        else
        {
            StatusText.Text = result.Error;
            HideLoading();
        }
    }

    private void GuestLogin_Click(object sender, RoutedEventArgs e)
    {
        // Continua sem autenticação
        NavigationService.NavigateTo<DashboardPage>();
    }

    private void ShowLoading(string message)
    {
        LoadingPanel.Visibility = Visibility.Visible;
        LoadingText.Text = message;
        GoogleLoginButton.IsEnabled = false;
        GitHubLoginButton.IsEnabled = false;
        GuestButton.IsEnabled = false;
    }

    private void HideLoading()
    {
        LoadingPanel.Visibility = Visibility.Collapsed;
        GoogleLoginButton.IsEnabled = true;
        GitHubLoginButton.IsEnabled = true;
        GuestButton.IsEnabled = true;
    }
}
