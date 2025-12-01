using Microsoft.UI.Xaml.Controls;

namespace ConectaBairro.Services;

public static class NavigationService
{
    private static Frame? _rootFrame;

    public static void Initialize(Frame frame)
    {
        _rootFrame = frame;
    }

    public static void NavigateTo<T>() where T : Page
    {
        _rootFrame?.Navigate(typeof(T));
    }

    public static void NavigateTo(Type pageType)
    {
        _rootFrame?.Navigate(pageType);
    }

    public static void GoBack()
    {
        if (_rootFrame?.CanGoBack == true)
        {
            _rootFrame.GoBack();
        }
    }

    public static bool CanGoBack => _rootFrame?.CanGoBack ?? false;
}
