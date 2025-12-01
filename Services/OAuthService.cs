using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Storage;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de autenticação OAuth 2.0
/// Suporta login com Google e GitHub
/// </summary>
public class OAuthService
{
    private static OAuthService? _instance;
    public static OAuthService Instance => _instance ??= new OAuthService();

    // Configurações OAuth (em produção, usar secrets)
    private const string GoogleClientId = "YOUR_GOOGLE_CLIENT_ID";
    private const string GitHubClientId = "YOUR_GITHUB_CLIENT_ID";
    private const string GitHubClientSecret = "YOUR_GITHUB_CLIENT_SECRET";

    public OAuthUser? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;
    public string? AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }

    public event EventHandler<OAuthUser>? UserLoggedIn;
    public event EventHandler? UserLoggedOut;

    private readonly HttpClient _httpClient = new();
    private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

    private OAuthService() { }

    /// <summary>
    /// Login com Google OAuth 2.0
    /// </summary>
    public async Task<OAuthResult> LoginWithGoogleAsync()
    {
        try
        {
            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            var authUri = new Uri(
                $"https://accounts.google.com/o/oauth2/v2/auth?" +
                $"client_id={GoogleClientId}&" +
                $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
                $"response_type=token&" +
                $"scope=openid%20email%20profile");

            var result = await WebAuthenticationBroker.AuthenticateAsync(
                WebAuthenticationOptions.None, authUri);

            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                AccessToken = ParseAccessToken(result.ResponseData ?? "");
                await SaveTokensAsync();
                await FetchGoogleUserInfoAsync();
                return new OAuthResult { Success = true, Provider = "Google" };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro no login Google: {ex.Message}");
        }

        return new OAuthResult { Success = false, Error = "Falha na autenticação Google" };
    }

    /// <summary>
    /// Login com GitHub OAuth 2.0
    /// </summary>
    public async Task<OAuthResult> LoginWithGitHubAsync()
    {
        try
        {
            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            var authUri = new Uri(
                $"https://github.com/login/oauth/authorize?" +
                $"client_id={GitHubClientId}&" +
                $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
                $"scope=user:email");

            var result = await WebAuthenticationBroker.AuthenticateAsync(
                WebAuthenticationOptions.None, authUri);

            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                var code = ParseCode(result.ResponseData ?? "");
                await ExchangeGitHubCodeAsync(code);
                return new OAuthResult { Success = true, Provider = "GitHub" };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro no login GitHub: {ex.Message}");
        }

        return new OAuthResult { Success = false, Error = "Falha na autenticação GitHub" };
    }

    private async Task ExchangeGitHubCodeAsync(string code)
    {
        var response = await _httpClient.PostAsync(
            "https://github.com/login/oauth/access_token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = GitHubClientId,
                ["client_secret"] = GitHubClientSecret,
                ["code"] = code
            }));

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            AccessToken = ParseAccessToken(content);
            await SaveTokensAsync();
            await FetchGitHubUserInfoAsync();
        }
    }

    private async Task FetchGoogleUserInfoAsync()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            
            var userInfo = await _httpClient.GetFromJsonAsync<GoogleUserInfo>(
                "https://www.googleapis.com/oauth2/v2/userinfo");

            if (userInfo != null)
            {
                CurrentUser = new OAuthUser
                {
                    Id = userInfo.Id ?? "",
                    Name = userInfo.Name ?? "",
                    Email = userInfo.Email ?? "",
                    Picture = userInfo.Picture ?? "",
                    Provider = "Google"
                };
                UserLoggedIn?.Invoke(this, CurrentUser);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao buscar info Google: {ex.Message}");
        }
    }

    private async Task FetchGitHubUserInfoAsync()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ConectaBairro/1.0");
            
            var userInfo = await _httpClient.GetFromJsonAsync<GitHubUserInfo>(
                "https://api.github.com/user");

            if (userInfo != null)
            {
                CurrentUser = new OAuthUser
                {
                    Id = userInfo.Id.ToString(),
                    Name = userInfo.Name ?? userInfo.Login ?? "",
                    Email = userInfo.Email ?? "",
                    Picture = userInfo.AvatarUrl ?? "",
                    Provider = "GitHub"
                };
                UserLoggedIn?.Invoke(this, CurrentUser);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao buscar info GitHub: {ex.Message}");
        }
    }

    /// <summary>
    /// Logout e limpeza de tokens
    /// </summary>
    public async Task LogoutAsync()
    {
        CurrentUser = null;
        AccessToken = null;
        RefreshToken = null;
        
        _localSettings.Values["oauth_access_token"] = "";
        _localSettings.Values["oauth_refresh_token"] = "";
        _localSettings.Values["oauth_provider"] = "";
        
        UserLoggedOut?.Invoke(this, EventArgs.Empty);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Restaura sessão salva
    /// </summary>
    public async Task<bool> RestoreSessionAsync()
    {
        try
        {
            AccessToken = _localSettings.Values["oauth_access_token"] as string;
            RefreshToken = _localSettings.Values["oauth_refresh_token"] as string;
            var provider = _localSettings.Values["oauth_provider"] as string;

            if (!string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(provider))
            {
                if (provider == "Google")
                    await FetchGoogleUserInfoAsync();
                else if (provider == "GitHub")
                    await FetchGitHubUserInfoAsync();
                
                return IsAuthenticated;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao restaurar sessão: {ex.Message}");
        }
        return false;
    }

    private async Task SaveTokensAsync()
    {
        if (!string.IsNullOrEmpty(AccessToken))
            _localSettings.Values["oauth_access_token"] = AccessToken;
        if (!string.IsNullOrEmpty(RefreshToken))
            _localSettings.Values["oauth_refresh_token"] = RefreshToken;
        if (CurrentUser != null)
            _localSettings.Values["oauth_provider"] = CurrentUser.Provider;
        await Task.CompletedTask;
    }

    private static string ParseAccessToken(string response)
    {
        // Parse from URL fragment or form data
        foreach (var part in response.Split(new[] { '&', '#' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var keyValue = part.Split('=');
            if (keyValue.Length == 2 && keyValue[0] == "access_token")
                return keyValue[1];
        }
        return "";
    }

    private static string ParseCode(string response)
    {
        foreach (var part in response.Split(new[] { '&', '?' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var keyValue = part.Split('=');
            if (keyValue.Length == 2 && keyValue[0] == "code")
                return keyValue[1];
        }
        return "";
    }
}

public class OAuthUser
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
}

public class OAuthResult
{
    public bool Success { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

public class GoogleUserInfo
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Picture { get; set; }
}

public class GitHubUserInfo
{
    public long Id { get; set; }
    public string? Login { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? AvatarUrl { get; set; }
}
