using System;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using ConectaBairro.Views;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço para gerenciar dados do perfil do usuário
/// Persiste dados localmente
/// </summary>
public class UserProfileService
{
    private static UserProfileService? _instance;
    public static UserProfileService Instance => _instance ??= new UserProfileService();

    private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
    private UserProfileData? _cachedProfile;

    public event EventHandler<UserProfileData>? ProfileUpdated;

    private UserProfileService() { }

    public async Task<UserProfileData> GetProfileAsync()
    {
        if (_cachedProfile != null) return _cachedProfile;

        try
        {
            var json = _localSettings.Values["user_profile"] as string;
            if (!string.IsNullOrEmpty(json))
            {
                _cachedProfile = JsonSerializer.Deserialize<UserProfileData>(json);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar perfil: {ex.Message}");
        }

        _cachedProfile ??= GetDefaultProfile();
        await Task.CompletedTask;
        return _cachedProfile;
    }

    /// <summary>
    /// Alias para GetProfileAsync - carrega perfil do storage
    /// </summary>
    public async Task<UserProfileData?> LoadProfileAsync()
    {
        return await GetProfileAsync();
    }

    public async Task SaveProfileAsync(UserProfileData profile)
    {
        try
        {
            var json = JsonSerializer.Serialize(profile);
            _localSettings.Values["user_profile"] = json;
            _cachedProfile = profile;
            ProfileUpdated?.Invoke(this, profile);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao salvar perfil: {ex.Message}");
        }
        await Task.CompletedTask;
    }

    public string GetInitials()
    {
        var profile = _cachedProfile ?? GetDefaultProfile();
        if (string.IsNullOrEmpty(profile.Name)) return "??";
        
        var parts = profile.Name.Split(' ');
        return parts.Length >= 2 
            ? $"{parts[0][0]}{parts[^1][0]}".ToUpper()
            : profile.Name[..Math.Min(2, profile.Name.Length)].ToUpper();
    }

    public string GetLocation()
    {
        var profile = _cachedProfile ?? GetDefaultProfile();
        return $"{profile.Neighborhood}, {profile.City}";
    }

    private static UserProfileData GetDefaultProfile() => new()
    {
        Name = "Paulo Silva",
        Email = "paulo@email.com",
        Phone = "(11) 98765-4321",
        Neighborhood = "Vila Mariana",
        City = "São Paulo",
        InterestTech = true,
        InterestBusiness = true,
        AlertsEnabled = true,
        OpportunitiesEnabled = true,
        MessagesEnabled = true
    };
}
