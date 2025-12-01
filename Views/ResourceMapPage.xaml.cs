using ConectaBairro.ViewModels;

namespace ConectaBairro.Views;

public sealed partial class ResourceMapPage : Page
{
    public ResourceMapPage()
    {
        this.InitializeComponent();
        this.DataContext = new ResourceMapViewModel();
    }
}
