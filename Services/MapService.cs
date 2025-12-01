using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de integração com Google Maps API
/// Fornece localização, cálculo de distâncias, navegação e busca de lugares
/// </summary>
public class MapService
{
    private static MapService? _instance;
    public static MapService Instance => _instance ??= new MapService();

    // Google Maps API Key - Carrega de variável de ambiente ou usa valor padrão
    // Em produção, configurar via: Environment.SetEnvironmentVariable("GOOGLE_MAPS_API_KEY", "sua_chave")
    private static readonly string GoogleMapsApiKey = 
        Environment.GetEnvironmentVariable("GOOGLE_MAPS_API_KEY") ?? "AIzaSyDmV_dwbLnuJGCFLvXnaKlrz0g7rzrUM1Q";
    
    private const string PlacesApiUrl = "https://maps.googleapis.com/maps/api/place";
    private const string DirectionsApiUrl = "https://maps.googleapis.com/maps/api/directions/json";
    private const string MapsJsUrl = "https://maps.googleapis.com/maps/api/js";

    private readonly HttpClient _httpClient = new();
    
    public BasicGeoposition? CurrentPosition { get; private set; }
    public event EventHandler<BasicGeoposition>? LocationChanged;

    private MapService() 
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "ConectaBairro/1.0");
    }

    /// <summary>
    /// Obtém URL do script do Google Maps para uso em WebView
    /// </summary>
    public static string GetMapsScriptUrl() => 
        $"{MapsJsUrl}?key={GoogleMapsApiKey}&libraries=places,directions";

    /// <summary>
    /// Verifica se a API Key está configurada
    /// </summary>
    public static bool IsApiKeyConfigured => 
        !string.IsNullOrEmpty(GoogleMapsApiKey) && GoogleMapsApiKey != "YOUR_GOOGLE_MAPS_API_KEY";

    /// <summary>
    /// Obtém a localização atual do usuário
    /// </summary>
    public async Task<BasicGeoposition?> GetCurrentLocationAsync()
    {
        try
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus != GeolocationAccessStatus.Allowed)
                return null;

            var geolocator = new Geolocator { DesiredAccuracyInMeters = 100 };
            var position = await geolocator.GetGeopositionAsync();
            
            CurrentPosition = position.Coordinate.Point.Position;
            LocationChanged?.Invoke(this, CurrentPosition.Value);

            return CurrentPosition;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao obter localização: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Calcula distância entre dois pontos usando fórmula de Haversine
    /// </summary>
    public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // Raio da Terra em km
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;

    /// <summary>
    /// Busca recursos próximos por categoria
    /// </summary>
    public async Task<List<NearbyResource>> GetNearbyResourcesAsync(string category, double radiusKm = 5)
    {
        var position = CurrentPosition ?? await GetCurrentLocationAsync();
        if (position == null) return new List<NearbyResource>();

        var allResources = GetMockResources();
        var nearby = new List<NearbyResource>();

        foreach (var resource in allResources)
        {
            if (!string.IsNullOrEmpty(category) && resource.Category != category)
                continue;

            var distance = CalculateDistance(
                position.Value.Latitude, position.Value.Longitude,
                resource.Latitude, resource.Longitude);

            if (distance <= radiusKm)
            {
                resource.DistanceKm = distance;
                nearby.Add(resource);
            }
        }

        return nearby.OrderBy(r => r.DistanceKm).ToList();
    }

    /// <summary>
    /// Abre o app de mapas nativo com navegação
    /// </summary>
    public async Task OpenNavigationAsync(double latitude, double longitude, string name)
    {
        try
        {
            var uri = new Uri($"bingmaps:?rtp=~pos.{latitude}_{longitude}_{Uri.EscapeDataString(name)}");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao abrir mapa: {ex.Message}");
        }
    }

    /// <summary>
    /// Busca lugares usando Google Places API
    /// </summary>
    public async Task<List<GooglePlace>> SearchPlacesAsync(string query, double lat, double lng, int radiusMeters = 5000)
    {
        try
        {
            var url = $"{PlacesApiUrl}/nearbysearch/json?location={lat},{lng}&radius={radiusMeters}&keyword={Uri.EscapeDataString(query)}&key={GoogleMapsApiKey}";
            var response = await _httpClient.GetFromJsonAsync<GooglePlacesResponse>(url);
            
            return response?.Results?.Select(r => new GooglePlace
            {
                PlaceId = r.PlaceId ?? "",
                Name = r.Name ?? "",
                Address = r.Vicinity ?? "",
                Latitude = r.Geometry?.Location?.Lat ?? 0,
                Longitude = r.Geometry?.Location?.Lng ?? 0,
                Rating = r.Rating,
                Types = r.Types ?? new List<string>()
            }).ToList() ?? new List<GooglePlace>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro Google Places: {ex.Message}");
            return new List<GooglePlace>();
        }
    }

    /// <summary>
    /// Obtém direções entre dois pontos via Google Directions API
    /// </summary>
    public async Task<RouteInfo?> GetDirectionsAsync(double originLat, double originLng, double destLat, double destLng, string mode = "driving")
    {
        try
        {
            var url = $"{DirectionsApiUrl}?origin={originLat},{originLng}&destination={destLat},{destLng}&mode={mode}&key={GoogleMapsApiKey}";
            var response = await _httpClient.GetFromJsonAsync<GoogleDirectionsResponse>(url);
            
            var route = response?.Routes?.FirstOrDefault();
            var leg = route?.Legs?.FirstOrDefault();
            
            if (leg != null)
            {
                return new RouteInfo
                {
                    Distance = leg.Distance?.Text ?? "",
                    Duration = leg.Duration?.Text ?? "",
                    StartAddress = leg.StartAddress ?? "",
                    EndAddress = leg.EndAddress ?? ""
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro Google Directions: {ex.Message}");
        }
        return null;
    }

    /// <summary>
    /// Abre Google Maps no navegador com rota
    /// </summary>
    public async Task OpenGoogleMapsRouteAsync(double destLat, double destLng, string? destName = null)
    {
        try
        {
            var url = $"https://www.google.com/maps/dir/?api=1&destination={destLat},{destLng}";
            if (!string.IsNullOrEmpty(destName))
                url += $"&destination_place_id={Uri.EscapeDataString(destName)}";
            
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao abrir Google Maps: {ex.Message}");
        }
    }

    /// <summary>
    /// Busca instituições de ensino técnico próximas
    /// </summary>
    public async Task<List<NearbyResource>> GetInstituicoesEnsinoAsync(double lat, double lng, double radiusKm = 10)
    {
        var instituicoes = new List<NearbyResource>
        {
            new() { Name = "SENAI Vila Leopoldina", Category = "Educação", Latitude = -23.5289, Longitude = -46.7195, Address = "Rua Jaguaré, 500", Type = "SENAI" },
            new() { Name = "SENAC Consolação", Category = "Educação", Latitude = -23.5505, Longitude = -46.6533, Address = "Rua Dr. Vila Nova, 228", Type = "SENAC" },
            new() { Name = "ETEC Parque da Juventude", Category = "Educação", Latitude = -23.5155, Longitude = -46.6270, Address = "Av. Cruzeiro do Sul, 2630", Type = "ETEC" },
            new() { Name = "IFSP Campus São Paulo", Category = "Educação", Latitude = -23.5290, Longitude = -46.6325, Address = "Rua Pedro Vicente, 625", Type = "IF" },
            new() { Name = "FATEC Ipiranga", Category = "Educação", Latitude = -23.5870, Longitude = -46.6080, Address = "Av. Nazaré, 2001", Type = "FATEC" }
        };

        var nearby = new List<NearbyResource>();
        foreach (var inst in instituicoes)
        {
            inst.DistanceKm = CalculateDistance(lat, lng, inst.Latitude, inst.Longitude);
            if (inst.DistanceKm <= radiusKm)
                nearby.Add(inst);
        }
        
        await Task.CompletedTask;
        return nearby.OrderBy(r => r.DistanceKm).ToList();
    }

    private static List<NearbyResource> GetMockResources() => new()
    {
        new NearbyResource { Name = "UBS Centro", Category = "Saúde", Latitude = -23.5505, Longitude = -46.6333, Address = "Rua da Saúde, 100" },
        new NearbyResource { Name = "Hospital Municipal", Category = "Saúde", Latitude = -23.5520, Longitude = -46.6350, Address = "Av. Principal, 500" },
        new NearbyResource { Name = "SENAI Centro", Category = "Educação", Latitude = -23.5480, Longitude = -46.6300, Address = "Rua Industrial, 200" },
        new NearbyResource { Name = "SENAC Paulista", Category = "Educação", Latitude = -23.5550, Longitude = -46.6400, Address = "Av. Paulista, 1000" },
        new NearbyResource { Name = "SINE Municipal", Category = "Trabalho", Latitude = -23.5490, Longitude = -46.6320, Address = "Rua do Emprego, 50" },
        new NearbyResource { Name = "CAT Centro", Category = "Trabalho", Latitude = -23.5530, Longitude = -46.6380, Address = "Praça Central, 10" },
        new NearbyResource { Name = "CRAS Centro", Category = "Social", Latitude = -23.5510, Longitude = -46.6340, Address = "Rua Social, 75" },
        new NearbyResource { Name = "CREAS Municipal", Category = "Social", Latitude = -23.5540, Longitude = -46.6360, Address = "Av. Cidadania, 300" }
    };
}

// Google Places API Response Models
public class GooglePlacesResponse
{
    public List<GooglePlaceResult>? Results { get; set; }
}

public class GooglePlaceResult
{
    public string? PlaceId { get; set; }
    public string? Name { get; set; }
    public string? Vicinity { get; set; }
    public GoogleGeometry? Geometry { get; set; }
    public double? Rating { get; set; }
    public List<string>? Types { get; set; }
}

public class GoogleGeometry
{
    public GoogleLocation? Location { get; set; }
}

public class GoogleLocation
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}

// Google Directions API Response Models
public class GoogleDirectionsResponse
{
    public List<GoogleRoute>? Routes { get; set; }
}

public class GoogleRoute
{
    public List<GoogleLeg>? Legs { get; set; }
}

public class GoogleLeg
{
    public GoogleTextValue? Distance { get; set; }
    public GoogleTextValue? Duration { get; set; }
    public string? StartAddress { get; set; }
    public string? EndAddress { get; set; }
}

public class GoogleTextValue
{
    public string? Text { get; set; }
    public int Value { get; set; }
}

// Custom Models
public class GooglePlace
{
    public string PlaceId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Rating { get; set; }
    public List<string> Types { get; set; } = new();
}

public class RouteInfo
{
    public string Distance { get; set; } = "";
    public string Duration { get; set; } = "";
    public string StartAddress { get; set; } = "";
    public string EndAddress { get; set; } = "";
}

public class NearbyResource
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // SENAI, SENAC, ETEC, IF, FATEC, UBS, etc.
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double DistanceKm { get; set; }
    public string DistanceFormatted => DistanceKm < 1 ? $"{DistanceKm * 1000:F0}m" : $"{DistanceKm:F1}km";
}
