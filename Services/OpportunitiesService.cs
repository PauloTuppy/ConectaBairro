using System.Diagnostics;
using System.Net.Http.Json;

namespace ConectaBairro.Services;

public record OpportunityProgram
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public string Provider { get; init; } = "";
    public string Program { get; init; } = "";
    public string Category { get; init; } = "";
    public decimal Salary { get; init; }
    public string Duration { get; init; } = "";
    public int Vacancies { get; init; }
    public string Location { get; init; } = "";
    public string Requirements { get; init; } = "";
    public DateTime Deadline { get; init; }
    public string Url { get; init; } = "";
    public bool IsOpen { get; init; } = true;
}

public class OpportunitiesService
{
    private static readonly HttpClient _httpClient = new();
    private const string BrasilApiUrl = "https://brasilapi.com.br/api";

    public static async Task<List<OpportunityProgram>> GetAllOpportunitiesAsync(string state = "SP")
    {
        var programs = new List<OpportunityProgram>();

        // Carregar dados de cada programa
        programs.AddRange(GetAutonomiaRendaPrograms(state));
        programs.AddRange(GetPronatecPrograms(state));
        programs.AddRange(GetSenacPrograms(state));
        programs.AddRange(GetSescPrograms(state));

        // Simular delay de API
        await Task.Delay(500);

        return programs.OrderByDescending(p => p.Salary).ToList();
    }

    public static async Task<string?> GetCityFromCepAsync(string cep)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<CepResponse>($"{BrasilApiUrl}/cep/v1/{cep}");
            return response?.City;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao buscar CEP: {ex.Message}");
            return null;
        }
    }

    private static List<OpportunityProgram> GetAutonomiaRendaPrograms(string state)
    {
        return new List<OpportunityProgram>
        {
            new()
            {
                Name = "Técnico em Manutenção Elétrica",
                Description = "Formação técnica completa em instalações elétricas residenciais e industriais. Inclui prática em laboratório.",
                Provider = "SENAI",
                Program = "Autonomia e Renda",
                Category = "Técnico",
                Salary = 858,
                Duration = "6 meses",
                Vacancies = 45,
                Location = $"São Paulo, {state}",
                Requirements = "Ensino Médio completo, idade entre 18-45 anos",
                Deadline = DateTime.Now.AddDays(30),
                Url = "https://www.gov.br/trabalho-e-emprego/pt-br/servicos/trabalhador/qualificacao-social-e-profissional"
            },
            new()
            {
                Name = "Instalador Hidráulico",
                Description = "Curso de instalações hidráulicas prediais com certificação. Aprenda a instalar e reparar sistemas de água e esgoto.",
                Provider = "SENAI",
                Program = "Autonomia e Renda",
                Category = "Técnico",
                Salary = 750,
                Duration = "4 meses",
                Vacancies = 35,
                Location = $"Campinas, {state}",
                Requirements = "Ensino Fundamental completo",
                Deadline = DateTime.Now.AddDays(25),
                Url = "https://www.gov.br/trabalho-e-emprego/pt-br/servicos/trabalhador/qualificacao-social-e-profissional"
            },
            new()
            {
                Name = "Operador de Empilhadeira",
                Description = "Habilitação para operação de empilhadeiras elétricas e a combustão. Inclui NR-11.",
                Provider = "SENAI",
                Program = "Autonomia e Renda",
                Category = "Logística",
                Salary = 1200,
                Duration = "3 meses",
                Vacancies = 60,
                Location = $"Guarulhos, {state}",
                Requirements = "CNH categoria B, Ensino Fundamental",
                Deadline = DateTime.Now.AddDays(20),
                Url = "https://www.gov.br/trabalho-e-emprego/pt-br/servicos/trabalhador/qualificacao-social-e-profissional"
            }
        };
    }

    private static List<OpportunityProgram> GetPronatecPrograms(string state)
    {
        return new List<OpportunityProgram>
        {
            new()
            {
                Name = "Programador Python",
                Description = "Aprenda programação do zero com Python. Desenvolvimento web, automação e análise de dados.",
                Provider = "IFSP",
                Program = "PRONATEC",
                Category = "Tecnologia",
                Salary = 0,
                Duration = "4 meses",
                Vacancies = 80,
                Location = $"São Paulo, {state}",
                Requirements = "Ensino Médio completo ou cursando",
                Deadline = DateTime.Now.AddDays(15),
                Url = "https://www.gov.br/mec/pt-br/pronatec"
            },
            new()
            {
                Name = "Assistente de Gestão",
                Description = "Formação em rotinas administrativas, gestão de pessoas e processos empresariais.",
                Provider = "ETEC",
                Program = "PRONATEC",
                Category = "Administrativo",
                Salary = 0,
                Duration = "3 meses",
                Vacancies = 120,
                Location = $"Santo André, {state}",
                Requirements = "Ensino Médio completo",
                Deadline = DateTime.Now.AddDays(22),
                Url = "https://www.gov.br/mec/pt-br/pronatec"
            },
            new()
            {
                Name = "Operador de Computador",
                Description = "Curso básico de informática: Windows, Office, Internet e noções de hardware.",
                Provider = "SENAC",
                Program = "PRONATEC",
                Category = "Tecnologia",
                Salary = 0,
                Duration = "2 meses",
                Vacancies = 150,
                Location = $"Osasco, {state}",
                Requirements = "Ensino Fundamental completo",
                Deadline = DateTime.Now.AddDays(18),
                Url = "https://www.gov.br/mec/pt-br/pronatec"
            }
        };
    }

    private static List<OpportunityProgram> GetSenacPrograms(string state)
    {
        return new List<OpportunityProgram>
        {
            new()
            {
                Name = "Chef de Cozinha",
                Description = "Formação completa em gastronomia. Técnicas culinárias, gestão de cozinha e empreendedorismo.",
                Provider = "SENAC",
                Program = "SENAC",
                Category = "Gastronomia",
                Salary = 1500,
                Duration = "8 meses",
                Vacancies = 25,
                Location = $"São Paulo, {state}",
                Requirements = "Ensino Médio completo, maior de 18 anos",
                Deadline = DateTime.Now.AddDays(35),
                Url = "https://www.sp.senac.br/cursos-livres"
            },
            new()
            {
                Name = "Desenvolvedor Web Full Stack",
                Description = "HTML, CSS, JavaScript, React, Node.js e banco de dados. Projeto final com portfólio.",
                Provider = "SENAC",
                Program = "SENAC",
                Category = "Tecnologia",
                Salary = 2500,
                Duration = "6 meses",
                Vacancies = 40,
                Location = $"São Paulo, {state}",
                Requirements = "Ensino Médio completo, conhecimento básico de informática",
                Deadline = DateTime.Now.AddDays(28),
                Url = "https://www.sp.senac.br/cursos-livres"
            }
        };
    }

    private static List<OpportunityProgram> GetSescPrograms(string state)
    {
        return new List<OpportunityProgram>
        {
            new()
            {
                Name = "Auxiliar de Farmácia",
                Description = "Formação para atuar em farmácias e drogarias. Dispensação, estoque e atendimento.",
                Provider = "SESC",
                Program = "SESC",
                Category = "Saúde",
                Salary = 900,
                Duration = "4 meses",
                Vacancies = 30,
                Location = $"São Paulo, {state}",
                Requirements = "Ensino Médio completo",
                Deadline = DateTime.Now.AddDays(20),
                Url = "https://www.sescsp.org.br/programacao"
            },
            new()
            {
                Name = "Cuidador de Idosos",
                Description = "Capacitação para cuidados com pessoas idosas. Saúde, higiene, alimentação e bem-estar.",
                Provider = "SESC",
                Program = "SESC",
                Category = "Saúde",
                Salary = 1100,
                Duration = "3 meses",
                Vacancies = 45,
                Location = $"Ribeirão Preto, {state}",
                Requirements = "Ensino Fundamental completo, maior de 21 anos",
                Deadline = DateTime.Now.AddDays(25),
                Url = "https://www.sescsp.org.br/programacao"
            }
        };
    }
}

public record CepResponse
{
    public string? Cep { get; init; }
    public string? State { get; init; }
    public string? City { get; init; }
    public string? Neighborhood { get; init; }
    public string? Street { get; init; }
}
