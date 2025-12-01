using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ConectaBairro.Models;
using ConectaBairro.MockData;
using System;
using System.Linq;
using CommunityToolkit.Mvvm.Input; // Added for RelayCommand

namespace ConectaBairro.ViewModels;

public partial class CoursesViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Course> allCourses = new();

    [ObservableProperty]
    private ObservableCollection<Course> filteredCourses = new();

    // Changed to FilterOption
    [ObservableProperty]
    private ObservableCollection<FilterOption> programFilters = new();

    // Changed to FilterOption
    [ObservableProperty]
    private ObservableCollection<FilterOption> durationFilters = new();

    [ObservableProperty]
    private decimal minStipend = 0;

    public CoursesViewModel()
    {
        // Load mock data for now
        var mockCourses = MockCourses.GetMockCourses();
        AllCourses = new ObservableCollection<Course>(mockCourses);
        
        // Populate program filters
        ProgramFilters.Add(new FilterOption { Name = "Todos", IsSelected = true }); // Default selected
        foreach (var programName in Enum.GetNames(typeof(ProgramType)))
        {
            ProgramFilters.Add(new FilterOption { Name = programName });
        }
        
        // Populate duration filters
        DurationFilters.Add(new FilterOption { Name = "Todos", IsSelected = true }); // Default selected
        DurationFilters.Add(new FilterOption { Name = "1-3 meses" });
        DurationFilters.Add(new FilterOption { Name = "3-6 meses" });
        DurationFilters.Add(new FilterOption { Name = "6+ meses" });

        ApplyFilters(); // Initial filter application
    }

    partial void OnMinStipendChanged(decimal value) => ApplyFilters();

    [RelayCommand]
    private void SelectProgramFilter(FilterOption selectedOption)
    {
        foreach (var option in ProgramFilters)
        {
            option.IsSelected = (option == selectedOption);
        }
        ApplyFilters();
    }

    [RelayCommand]
    private void SelectDurationFilter(FilterOption selectedOption)
    {
        foreach (var option in DurationFilters)
        {
            option.IsSelected = (option == selectedOption);
        }
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var query = AllCourses.AsEnumerable();

        // Apply program filter
        var selectedProgram = ProgramFilters.FirstOrDefault(f => f.IsSelected)?.Name;
        if (selectedProgram != "Todos" && selectedProgram != null)
        {
            ProgramType programType = (ProgramType)Enum.Parse(typeof(ProgramType), selectedProgram);
            query = query.Where(c => c.Program == programType);
        }

        // Apply duration filter
        var selectedDuration = DurationFilters.FirstOrDefault(f => f.IsSelected)?.Name;
        if (selectedDuration != "Todos" && selectedDuration != null)
        {
            // Simple logic for duration filtering - needs to be robust
            if (selectedDuration == "1-3 meses")
            {
                query = query.Where(c => int.TryParse(c.Duration.Split(' ')[0], out int months) && months >= 1 && months <=3 );
            }
            else if (selectedDuration == "3-6 meses")
            {
                query = query.Where(c => int.TryParse(c.Duration.Split(' ')[0], out int months) && months > 3 && months <=6);
            }
            else if (selectedDuration == "6+ meses")
            {
                query = query.Where(c => int.TryParse(c.Duration.Split(' ')[0], out int months) && months > 6);
            }
        }

        if (MinStipend > 0)
        {
            query = query.Where(c => c.Stipend >= MinStipend);
        }

        FilteredCourses = new ObservableCollection<Course>(query);
    }
}
