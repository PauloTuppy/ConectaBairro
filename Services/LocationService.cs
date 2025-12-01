using System.Diagnostics;

namespace ConectaBairro.Services;

public record GeoLocation(double Latitude, double Longitude);

public record LocalResource
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Name { get; init; } = "";
    public string Type { get; init; } = "";
    public string Address { get; init; } = "";
    public GeoLocation Location { get; init; } = new(0, 0);
    public string Phone { get; init; } = "";
    public string Hours { get; init; } = "";
    public double Distance { get; init; }
}

public class LocationService
{
    private static LocationService? _instance;
    public static LocationService Instance => _instance ??= new LocationService();

    // Localização padrão: Vila Mariana, São Paulo
    public GeoLocation DefaultLocation { get; } = new(-23.5874, -46.6426);
    public GeoLocation? CurrentLocation { get; private set; }

    public double CalculateDistance(GeoLocation from, GeoLocation to)
    {
        const double R = 6371; // Raio da Terra em km
        var dLat = ToRad(to.Latitude - from.Latitude);
        var dLon = ToRad(to.Longitude - from.Longitude);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(from.Latitude)) * Math.Cos(ToRad(to.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRad(double deg) => deg * Math.PI / 180;

    public List<LocalResource> GetNearbyResources(string? type = null)
    {
        var resources = GetMockResources();
        var userLocation = CurrentLocation ?? DefaultLocation;

        var result = resources
            .Select(r => r with { Distance = CalculateDistance(userLocation, r.Location) })
            .OrderBy(r => r.Distance);

        if (!string.IsNullOrEmpty(type))
        {
            return result.Where(r => r.Type == type).ToList();
        }

        return result.ToList();
    }

    private static List<LocalResource> GetMockResources()
    {
        return new List<LocalResource>
        {
            new()
            {
                Name = "UBS Vila Mariana",
                Type = "Saúde",
                Address = "Rua Domingos de Morais, 2564",
                Location = new(-23.5891, -46.6389),
                Phone = "(11) 5549-1234",
                Hours = "7h às 19h"
            },
            new()
            {
                Name = "ETEC São Paulo",
                Type = "Educação",
                Address = "Praça Cel. Fernando Prestes, 74",
                Location = new(-23.5432, -46.6339),
                Phone = "(11) 3327-3000",
                Hours = "7h às 22h"
            },
            new()
            {
                Name = "CRAS Vila Mariana",
                Type = "Social",
                Address = "Rua Vergueiro, 3456",
                Location = new(-23.5812, -46.6401),
                Phone = "(11) 5083-4567",
                Hours = "8h às 17h"
            },
            new()
            {
                Name = "Biblioteca Alceu Amoroso Lima",
                Type = "Cultura",
                Address = "Rua Henrique Schaumann, 777",
                Location = new(-23.5634, -46.6712),
                Phone = "(11) 3082-5023",
                Hours = "9h às 21h"
            },
            new()
            {
                Name = "SENAC Consolação",
                Type = "Educação",
                Address = "Rua Dr. Vila Nova, 228",
                Location = new(-23.5512, -46.6523),
                Phone = "(11) 3236-2000",
                Hours = "8h às 22h"
            },
            new()
            {
                Name = "Poupatempo Sé",
                Type = "Serviços",
                Address = "Praça do Carmo, s/n",
                Location = new(-23.5489, -46.6345),
                Phone = "0800 772 3633",
                Hours = "7h às 19h"
            }
        };
    }
}
