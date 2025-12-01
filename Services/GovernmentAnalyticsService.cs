using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConectaBairro.Services;

/// <summary>
/// Analytics para governos - Dashboard de impacto social
/// </summary>
public class GovernmentAnalyticsService
{
    private static GovernmentAnalyticsService? _instance;
    public static GovernmentAnalyticsService Instance => _instance ??= new GovernmentAnalyticsService();

    private GovernmentAnalyticsService() { }

    public async Task<ImpactDashboard> GetImpactDashboardAsync(string state = "SP")
    {
        await Task.Delay(100);
        return new ImpactDashboard
        {
            State = state,
            GeneratedAt = DateTime.Now,
            Economic = new EconomicImpact
            {
                ActiveUsers = 125000,
                TotalIncomeGenerated = 52000000,
                AverageIncomePerUser = 6341,
                JobsCreated = 8200,
                BusinessesStarted = 450
            },
            Social = new SocialImpact
            {
                SkillsTrained = 45000,
                EducationHours = 450000,
                CommunityConnections = 125000,
                AlertsProcessed = 23000,
                FamiliesImpacted = 32000
            },
            Demographics = new Demographics
            {
                AverageAge = 35,
                FemalePercentage = 38,
                MalePercentage = 62,
                TopCities = new() { "São Paulo", "Campinas", "Santos", "Ribeirão Preto" },
                IncomeDistribution = new() { { "0-1SM", 25 }, { "1-2SM", 35 }, { "2-3SM", 25 }, { "3+SM", 15 } }
            },
            MonthlyGrowth = new()
            {
                { "Jan", 85000 }, { "Fev", 95000 }, { "Mar", 105000 }, { "Abr", 125000 }
            }
        };
    }

    public async Task<List<PolicyRecommendation>> GetPolicyRecommendationsAsync()
    {
        await Task.Delay(50);
        return new()
        {
            new() { Title = "Expandir cursos de TI", Description = "Alta demanda por programação na região", Priority = "Alta", EstimatedImpact = 15000 },
            new() { Title = "Parcerias com empresas locais", Description = "Aumentar vagas de emprego formal", Priority = "Média", EstimatedImpact = 8000 },
            new() { Title = "Programa de microcrédito", Description = "Apoiar empreendedores da plataforma", Priority = "Alta", EstimatedImpact = 5000 }
        };
    }

    public async Task<byte[]> ExportReportPdfAsync(string state)
    {
        // Em produção, usar biblioteca como QuestPDF ou iTextSharp
        await Task.Delay(200);
        return Array.Empty<byte>();
    }
}

public class ImpactDashboard
{
    public string State { get; set; } = "";
    public DateTime GeneratedAt { get; set; }
    public EconomicImpact Economic { get; set; } = new();
    public SocialImpact Social { get; set; } = new();
    public Demographics Demographics { get; set; } = new();
    public Dictionary<string, int> MonthlyGrowth { get; set; } = new();
}

public class EconomicImpact
{
    public int ActiveUsers { get; set; }
    public decimal TotalIncomeGenerated { get; set; }
    public decimal AverageIncomePerUser { get; set; }
    public int JobsCreated { get; set; }
    public int BusinessesStarted { get; set; }
}

public class SocialImpact
{
    public int SkillsTrained { get; set; }
    public int EducationHours { get; set; }
    public int CommunityConnections { get; set; }
    public int AlertsProcessed { get; set; }
    public int FamiliesImpacted { get; set; }
}

public class Demographics
{
    public int AverageAge { get; set; }
    public int FemalePercentage { get; set; }
    public int MalePercentage { get; set; }
    public List<string> TopCities { get; set; } = new();
    public Dictionary<string, int> IncomeDistribution { get; set; } = new();
}

public class PolicyRecommendation
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Priority { get; set; } = "";
    public int EstimatedImpact { get; set; }
}
