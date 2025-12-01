using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de parcerias com Universidades, Empresas, ONGs e Governo
/// </summary>
public class PartnerService
{
    private static PartnerService? _instance;
    public static PartnerService Instance => _instance ??= new PartnerService();

    private readonly List<Partner> _partners = new()
    {
        new() { Id = Guid.NewGuid(), Name = "USP", Type = PartnerType.University, Status = PartnerStatus.Active, OpportunitiesCount = 45, UsersReached = 12000 },
        new() { Id = Guid.NewGuid(), Name = "SENAI", Type = PartnerType.University, Status = PartnerStatus.Active, OpportunitiesCount = 120, UsersReached = 35000 },
        new() { Id = Guid.NewGuid(), Name = "Magazine Luiza", Type = PartnerType.Company, Status = PartnerStatus.Active, OpportunitiesCount = 28, UsersReached = 8500 },
        new() { Id = Guid.NewGuid(), Name = "Gerando Falcões", Type = PartnerType.NGO, Status = PartnerStatus.Active, OpportunitiesCount = 15, UsersReached = 5200 },
        new() { Id = Guid.NewGuid(), Name = "Prefeitura SP", Type = PartnerType.Government, Status = PartnerStatus.Active, OpportunitiesCount = 32, UsersReached = 18000 }
    };

    private PartnerService() { }

    public async Task<List<Partner>> GetAllPartnersAsync()
    {
        await Task.Delay(50);
        return _partners;
    }

    public async Task<List<Partner>> GetPartnersByTypeAsync(PartnerType type)
    {
        await Task.Delay(50);
        return _partners.FindAll(p => p.Type == type);
    }

    public async Task<Partner?> RegisterPartnerAsync(Partner partner)
    {
        partner.Id = Guid.NewGuid();
        partner.Status = PartnerStatus.Pending;
        partner.CreatedAt = DateTime.Now;
        _partners.Add(partner);
        await Task.CompletedTask;
        return partner;
    }

    public async Task<PartnerStats> GetPartnerStatsAsync(Guid partnerId)
    {
        var partner = _partners.Find(p => p.Id == partnerId);
        await Task.Delay(50);
        return new PartnerStats
        {
            PartnerId = partnerId,
            TotalOpportunities = partner?.OpportunitiesCount ?? 0,
            TotalUsersReached = partner?.UsersReached ?? 0,
            ConversionRate = 12.5,
            RevenueGenerated = 45000
        };
    }
}

public class Partner
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public PartnerType Type { get; set; }
    public PartnerStatus Status { get; set; }
    public string ContactEmail { get; set; } = "";
    public string LogoUrl { get; set; } = "";
    public int OpportunitiesCount { get; set; }
    public int UsersReached { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PartnerStats
{
    public Guid PartnerId { get; set; }
    public int TotalOpportunities { get; set; }
    public int TotalUsersReached { get; set; }
    public double ConversionRate { get; set; }
    public decimal RevenueGenerated { get; set; }
}

public enum PartnerType { University, Company, NGO, Government }
public enum PartnerStatus { Pending, Active, Inactive, Suspended }
