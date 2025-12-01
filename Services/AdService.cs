using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de anúncios patrocinados
/// Integração com Google AdMob
/// </summary>
public class AdService
{
    private static AdService? _instance;
    public static AdService Instance => _instance ??= new AdService();

    public event EventHandler<AdClick>? AdClicked;

    // Google AdMob IDs (usar IDs de teste em desenvolvimento)
    private const string BannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    private const string InterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";

    private AdService() { }

    public async Task<List<SponsoredAd>> GetSponsoredAdsAsync()
    {
        await Task.Delay(100);
        return new List<SponsoredAd>
        {
            new() {
                Id = Guid.NewGuid(),
                Title = "Técnico Elétrico - Urgente!",
                Description = "Empresa de energia procura técnico. Salário R$ 3.500",
                Company = "EletroSP Brasil",
                Link = "https://example.com/job/1",
                Type = AdType.JobOpportunity,
                Budget = 1000, SpentToday = 450, Impressions = 15000, Clicks = 342
            },
            new() {
                Id = Guid.NewGuid(),
                Title = "Curso Gratuito de Programação",
                Description = "SENAI oferece 500 vagas para curso de TI",
                Company = "SENAI São Paulo",
                Link = "https://example.com/course/1",
                Type = AdType.Course,
                Budget = 2000, SpentToday = 800, Impressions = 25000, Clicks = 890
            }
        };
    }

    public void TrackImpression(Guid adId) =>
        System.Diagnostics.Debug.WriteLine($"Ad Impression: {adId}");

    public void TrackClick(Guid adId, string userId)
    {
        AdClicked?.Invoke(this, new AdClick { AdId = adId, UserId = userId, ClickedAt = DateTime.Now });
        System.Diagnostics.Debug.WriteLine($"Ad Click: {adId} by {userId}");
    }

    public double CalculateCTR(int impressions, int clicks) =>
        impressions > 0 ? (double)clicks / impressions * 100 : 0;
}

public class SponsoredAd
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Company { get; set; } = "";
    public string Link { get; set; } = "";
    public AdType Type { get; set; }
    public decimal Budget { get; set; }
    public decimal SpentToday { get; set; }
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public double CTR => Impressions > 0 ? (double)Clicks / Impressions * 100 : 0;
}

public class AdClick
{
    public Guid AdId { get; set; }
    public string UserId { get; set; } = "";
    public DateTime ClickedAt { get; set; }
}

public enum AdType { JobOpportunity, Course, Service, Event, Government }
