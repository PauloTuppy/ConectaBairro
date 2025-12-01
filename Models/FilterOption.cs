using CommunityToolkit.Mvvm.ComponentModel;

namespace ConectaBairro.Models;

public partial class FilterOption : ObservableObject
{
    public string Name { get; set; } = "";

    [ObservableProperty]
    private bool isSelected = false;
}
