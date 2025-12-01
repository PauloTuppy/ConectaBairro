using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using ConectaBairro.Services;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ConectaBairro.Views;

public sealed partial class EditProfilePage : Page
{
    private string? _selectedPhotoPath;

    public EditProfilePage()
    {
        InitializeComponent();
        LoadExistingProfile();
    }

    private async void LoadExistingProfile()
    {
        var profile = await UserProfileService.Instance.LoadProfileAsync();
        if (profile != null)
        {
            NameInput.Text = profile.Name;
            EmailInput.Text = profile.Email;
            PhoneInput.Text = profile.Phone;
            NeighborhoodInput.Text = profile.Neighborhood;
            CityInput.Text = profile.City;
            InterestTech.IsChecked = profile.InterestTech;
            InterestHealth.IsChecked = profile.InterestHealth;
            InterestBusiness.IsChecked = profile.InterestBusiness;
            InterestEducation.IsChecked = profile.InterestEducation;
            InterestSocial.IsChecked = profile.InterestSocial;
            AlertsToggle.IsOn = profile.AlertsEnabled;
            OpportunitiesToggle.IsOn = profile.OpportunitiesEnabled;
            MessagesToggle.IsOn = profile.MessagesEnabled;

            // Carregar foto se existir
            if (!string.IsNullOrEmpty(profile.PhotoPath))
            {
                await LoadPhotoAsync(profile.PhotoPath);
            }

            UpdateAvatarInitials(profile.Name);
        }
    }

    private async void ChangePhoto_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var picker = new FileOpenPicker();
            
            // Configurar o picker para a janela atual
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            picker.FileTypeFilter.Add(".gif");

            var file = await picker.PickSingleFileAsync();
            
            if (file != null)
            {
                // Copiar para pasta local do app
                var localFolder = ApplicationData.Current.LocalFolder;
                var profileFolder = await localFolder.CreateFolderAsync("ProfilePhotos", CreationCollisionOption.OpenIfExists);
                var newFile = await file.CopyAsync(profileFolder, $"profile_{System.DateTime.Now.Ticks}{file.FileType}", NameCollisionOption.ReplaceExisting);
                
                _selectedPhotoPath = newFile.Path;
                
                // Carregar e mostrar a imagem
                await LoadPhotoAsync(newFile.Path);
                
                PhotoStatusText.Text = $"✓ Foto selecionada: {file.Name}";
            }
        }
        catch (System.Exception ex)
        {
            PhotoStatusText.Text = $"Erro ao selecionar foto: {ex.Message}";
            PhotoStatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
        }
    }

    private async System.Threading.Tasks.Task LoadPhotoAsync(string path)
    {
        try
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            using var stream = await file.OpenAsync(FileAccessMode.Read);
            
            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(stream);
            
            AvatarImageBrush.ImageSource = bitmap;
            AvatarImageBorder.Visibility = Visibility.Visible;
        }
        catch
        {
            // Se falhar ao carregar, mantém as iniciais
            AvatarImageBorder.Visibility = Visibility.Collapsed;
        }
    }

    private void UpdateAvatarInitials(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var parts = name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            var initials = parts.Length >= 2
                ? $"{parts[0][0]}{parts[^1][0]}"
                : name[..System.Math.Min(2, name.Length)];
            AvatarInitials.Text = initials.ToUpper();
        }
    }

    private async void Save_Click(object sender, RoutedEventArgs e)
    {
        var profile = new UserProfileData
        {
            Name = NameInput.Text,
            Email = EmailInput.Text,
            Phone = PhoneInput.Text,
            Neighborhood = NeighborhoodInput.Text,
            City = CityInput.Text,
            PhotoPath = _selectedPhotoPath ?? "",
            InterestTech = InterestTech.IsChecked ?? false,
            InterestHealth = InterestHealth.IsChecked ?? false,
            InterestBusiness = InterestBusiness.IsChecked ?? false,
            InterestEducation = InterestEducation.IsChecked ?? false,
            InterestSocial = InterestSocial.IsChecked ?? false,
            AlertsEnabled = AlertsToggle.IsOn,
            OpportunitiesEnabled = OpportunitiesToggle.IsOn,
            MessagesEnabled = MessagesToggle.IsOn
        };

        await UserProfileService.Instance.SaveProfileAsync(profile);
        
        UpdateAvatarInitials(profile.Name);
        
        NavigationService.GoBack();
    }

    private void GoBack_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}

public class UserProfileData
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Neighborhood { get; set; } = "";
    public string City { get; set; } = "";
    public string PhotoPath { get; set; } = "";
    public bool InterestTech { get; set; }
    public bool InterestHealth { get; set; }
    public bool InterestBusiness { get; set; }
    public bool InterestEducation { get; set; }
    public bool InterestSocial { get; set; }
    public bool AlertsEnabled { get; set; } = true;
    public bool OpportunitiesEnabled { get; set; } = true;
    public bool MessagesEnabled { get; set; } = true;
}
