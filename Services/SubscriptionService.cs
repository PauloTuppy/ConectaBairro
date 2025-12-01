using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de assinaturas Premium
/// Integração com Stripe para pagamentos
/// </summary>
public class SubscriptionService
{
    private static SubscriptionService? _instance;
    public static SubscriptionService Instance => _instance ??= new SubscriptionService();

    public event EventHandler<SubscriptionChanged>? OnSubscriptionChanged;

    public static readonly List<SubscriptionPlan> AvailablePlans = new()
    {
        new() { Id = "free", Name = "Gratuito", Price = 0, ChatLimit = 5, HasAds = true, HasCertificates = false, HasAnalytics = false },
        new() { Id = "basic", Name = "Básico", Price = 9.99m, ChatLimit = 50, HasAds = false, HasCertificates = true, HasAnalytics = false },
        new() { Id = "pro", Name = "Pro", Price = 19.99m, ChatLimit = -1, HasAds = false, HasCertificates = true, HasAnalytics = true },
        new() { Id = "enterprise", Name = "Enterprise", Price = 99.99m, ChatLimit = -1, HasAds = false, HasCertificates = true, HasAnalytics = true, HasApiAccess = true }
    };

    private UserSubscription? _currentSubscription;

    private SubscriptionService() { }

    public async Task<UserSubscription> GetCurrentSubscriptionAsync(string userId)
    {
        await Task.Delay(50);
        return _currentSubscription ?? new UserSubscription { UserId = userId, PlanId = "free", Status = SubscriptionStatus.Active };
    }

    public async Task<bool> SubscribeAsync(string userId, string planId, string paymentMethodId)
    {
        try
        {
            // Simula integração Stripe
            await Task.Delay(500);
            
            _currentSubscription = new UserSubscription
            {
                UserId = userId,
                PlanId = planId,
                Status = SubscriptionStatus.Active,
                StartDate = DateTime.Now,
                NextBillingDate = DateTime.Now.AddMonths(1)
            };

            OnSubscriptionChanged?.Invoke(this, new SubscriptionChanged { UserId = userId, NewPlanId = planId });
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> CancelSubscriptionAsync(string userId)
    {
        if (_currentSubscription != null)
        {
            _currentSubscription.Status = SubscriptionStatus.Cancelled;
            _currentSubscription.CancelledAt = DateTime.Now;
            OnSubscriptionChanged?.Invoke(this, new SubscriptionChanged { UserId = userId, NewPlanId = "free" });
        }
        await Task.CompletedTask;
        return true;
    }

    public bool CanSendMessage(string planId, int messagesSentThisMonth)
    {
        var plan = AvailablePlans.Find(p => p.Id == planId);
        return plan?.ChatLimit == -1 || messagesSentThisMonth < (plan?.ChatLimit ?? 5);
    }

    public bool HasFeature(string planId, string feature) => feature switch
    {
        "certificates" => planId != "free",
        "analytics" => planId is "pro" or "enterprise",
        "api" => planId == "enterprise",
        "no_ads" => planId != "free",
        _ => false
    };
}

public class SubscriptionPlan
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int ChatLimit { get; set; } // -1 = ilimitado
    public bool HasAds { get; set; }
    public bool HasCertificates { get; set; }
    public bool HasAnalytics { get; set; }
    public bool HasApiAccess { get; set; }
}

public class UserSubscription
{
    public string UserId { get; set; } = "";
    public string PlanId { get; set; } = "free";
    public SubscriptionStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public DateTime? CancelledAt { get; set; }
}

public class SubscriptionChanged
{
    public string UserId { get; set; } = "";
    public string NewPlanId { get; set; } = "";
}

public enum SubscriptionStatus { Active, Cancelled, Expired, PastDue }
