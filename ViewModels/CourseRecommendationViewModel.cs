using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConectaBairro.Models;
using ConectaBairro.MockData;

namespace ConectaBairro.ViewModels;

public partial class CourseRecommendationViewModel : ObservableObject
{
    [ObservableProperty]
    private ImmutableList<Course> allCourses = ImmutableList<Course>.Empty;

    [ObservableProperty]
    private ImmutableList<Course> recommendedCourses = ImmutableList<Course>.Empty;

    [ObservableProperty]
    private ImmutableList<Course> searchResults = ImmutableList<Course>.Empty;

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string? errorMessage = null;

    private readonly Services.BrasilAPIService _brasilApi = new();
    private readonly Services.ICourseRepository _courseRepository = new Services.CourseRepository();

    public CourseRecommendationViewModel()
    {
        LoadInitialData();
    }

    [RelayCommand]
    public async Task InitializeUserProfile(string cep)
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            // Busca localização pelo CEP
            var (state, city, success) = await _brasilApi.GetLocationByCEP(cep);
            
            if (success)
            {
                // Filtra cursos pela localização do usuário
                var userLocation = city;
                var relevantCourses = AllCourses
                    .Where(c => c.Location.Contains(state) || c.Location.Contains(city))
                    .ToImmutableList();
                
                RecommendedCourses = relevantCourses;
            }
            else
            {
                ErrorMessage = "CEP não encontrado. Tente novamente.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao localizar: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void LoadInitialData()
    {
        try
        {
            var courses = await _courseRepository.GetAllAsync();
            AllCourses = courses.ToImmutableList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar cursos: {ex.Message}");
            // Fallback para mock data se houver erro
            AllCourses = MockCourses.GetMockCourses().ToImmutableList();
        }
    }

    [RelayCommand]
    public async Task SearchCourses(string? searchTerm = null)
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var results = await _courseRepository.SearchAsync(searchTerm);
            SearchResults = results;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao buscar: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task GetRecommendations(UserProfile userProfile)
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var recommendations = await _courseRepository.GetRecommendedAsync(userProfile);
            RecommendedCourses = recommendations;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao gerar recomendações: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
