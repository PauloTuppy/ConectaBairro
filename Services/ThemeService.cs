namespace ConectaBairro.Services;

public enum AppTheme
{
    Light,
    Dark,
    System
}

public class ThemeService
{
    private static ThemeService? _instance;
    public static ThemeService Instance => _instance ??= new ThemeService();

    private AppTheme _currentTheme = AppTheme.Light;
    public AppTheme CurrentTheme => _currentTheme;
    public bool IsDarkMode => _currentTheme == AppTheme.Dark;

    public event Action<AppTheme>? ThemeChanged;

    // Cores do tema Light
    public static class LightColors
    {
        public const string Background = "#F0F4F8";
        public const string Surface = "#FFFFFF";
        public const string Primary = "#0078D4";
        public const string Secondary = "#107C10";
        public const string Text = "#1F2937";
        public const string TextSecondary = "#6B7280";
        public const string Border = "#E5E7EB";
    }

    // Cores do tema Dark
    public static class DarkColors
    {
        public const string Background = "#0F172A";
        public const string Surface = "#1E293B";
        public const string Primary = "#3B82F6";
        public const string Secondary = "#22C55E";
        public const string Text = "#F1F5F9";
        public const string TextSecondary = "#94A3B8";
        public const string Border = "#334155";
    }

    public void SetTheme(AppTheme theme)
    {
        _currentTheme = theme;
        ThemeChanged?.Invoke(theme);
        SaveThemePreference(theme);
    }

    public void ToggleTheme()
    {
        SetTheme(_currentTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light);
    }

    public string GetColor(string colorName)
    {
        return _currentTheme == AppTheme.Dark
            ? colorName switch
            {
                "Background" => DarkColors.Background,
                "Surface" => DarkColors.Surface,
                "Primary" => DarkColors.Primary,
                "Secondary" => DarkColors.Secondary,
                "Text" => DarkColors.Text,
                "TextSecondary" => DarkColors.TextSecondary,
                "Border" => DarkColors.Border,
                _ => DarkColors.Text
            }
            : colorName switch
            {
                "Background" => LightColors.Background,
                "Surface" => LightColors.Surface,
                "Primary" => LightColors.Primary,
                "Secondary" => LightColors.Secondary,
                "Text" => LightColors.Text,
                "TextSecondary" => LightColors.TextSecondary,
                "Border" => LightColors.Border,
                _ => LightColors.Text
            };
    }

    private void SaveThemePreference(AppTheme theme)
    {
        // Salvar preferência localmente
        Windows.Storage.ApplicationData.Current.LocalSettings.Values["AppTheme"] = theme.ToString();
    }

    public void LoadThemePreference()
    {
        var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        if (settings.Values.TryGetValue("AppTheme", out var themeValue))
        {
            if (Enum.TryParse<AppTheme>(themeValue?.ToString(), out var theme))
            {
                _currentTheme = theme;
            }
        }
    }
}
