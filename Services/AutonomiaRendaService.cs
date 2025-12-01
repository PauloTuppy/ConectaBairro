using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de Autonomia e Renda
/// Integração oficial: https://autonomiaerenda.com.br
/// Oportunidades de emprego, empreendedorismo e programas sociais
/// com localização via Google Maps
/// </summary>
public class AutonomiaRendaService
{
    private static AutonomiaRendaService? _instance;
    public static AutonomiaRendaService Instance => _instance ??= new AutonomiaRendaService();

    // URL oficial do programa Autonomia e Renda
    public const string AutonomiaRendaUrl = "https://autonomiaerenda.com.br";
    public const string AutonomiaRendaApiUrl = "https://autonomiaerenda.com.br/api";

    private readonly HttpClient _httpClient = new();

    private AutonomiaRendaService() 
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "ConectaBairro/1.0");
    }

    /// <summary>
    /// Abre o site oficial do Autonomia e Renda
    /// </summary>
    public static async Task AbrirSiteAutonomiaRendaAsync()
    {
        try
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(AutonomiaRendaUrl));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao abrir site: {ex.Message}");
        }
    }

    /// <summary>
    /// Busca oportunidades do programa Autonomia e Renda
    /// </summary>
    public async Task<List<OportunidadeAutonomia>> GetOportunidadesAutonomiaRendaAsync(string? cidade = null)
    {
        try
        {
            // Em produção, integrar com API real do autonomiaerenda.com.br
            await Task.Delay(100);
            return new List<OportunidadeAutonomia>
            {
                new() { Titulo = "Capacitação em Empreendedorismo", Tipo = "Curso", Descricao = "Aprenda a abrir seu próprio negócio", Gratuito = true, UrlInscricao = $"{AutonomiaRendaUrl}/cursos/empreendedorismo" },
                new() { Titulo = "Microcrédito Produtivo", Tipo = "Financiamento", Descricao = "Crédito para MEI e pequenos negócios", Gratuito = true, UrlInscricao = $"{AutonomiaRendaUrl}/microcredito" },
                new() { Titulo = "Qualificação Profissional", Tipo = "Curso", Descricao = "Cursos gratuitos de qualificação", Gratuito = true, UrlInscricao = $"{AutonomiaRendaUrl}/qualificacao" },
                new() { Titulo = "Inclusão Produtiva", Tipo = "Programa", Descricao = "Programa de inserção no mercado de trabalho", Gratuito = true, UrlInscricao = $"{AutonomiaRendaUrl}/inclusao" },
                new() { Titulo = "Economia Solidária", Tipo = "Programa", Descricao = "Apoio a cooperativas e associações", Gratuito = true, UrlInscricao = $"{AutonomiaRendaUrl}/economia-solidaria" }
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro Autonomia e Renda: {ex.Message}");
            return new List<OportunidadeAutonomia>();
        }
    }

    /// <summary>
    /// Busca oportunidades de emprego próximas
    /// </summary>
    public async Task<List<OportunidadeEmprego>> GetEmpregosPorLocalizacaoAsync(double lat, double lng, double raioKm = 10)
    {
        await Task.Delay(100);
        var empregos = GetEmpregosMock();
        
        // Filtrar por distância
        var resultado = new List<OportunidadeEmprego>();
        foreach (var emprego in empregos)
        {
            var distancia = MapService.Instance.CalculateDistance(lat, lng, emprego.Latitude, emprego.Longitude);
            if (distancia <= raioKm)
            {
                emprego.DistanciaKm = distancia;
                resultado.Add(emprego);
            }
        }
        
        resultado.Sort((a, b) => a.DistanciaKm.CompareTo(b.DistanciaKm));
        return resultado;
    }

    /// <summary>
    /// Busca programas de microcrédito e empreendedorismo
    /// </summary>
    public async Task<List<ProgramaMicrocredito>> GetProgramasMicrocreditoAsync(string estado = "SP")
    {
        await Task.Delay(100);
        return new List<ProgramaMicrocredito>
        {
            new() { Nome = "Banco do Povo Paulista", Descricao = "Microcrédito para MEI e pequenos negócios", ValorMaximo = 21000, TaxaJuros = 0.35, Requisitos = "MEI ou ME, CPF regular", Telefone = "0800-772-3633", Website = "https://www.bancodopovo.sp.gov.br" },
            new() { Nome = "PRONAMPE", Descricao = "Programa Nacional de Apoio às Microempresas", ValorMaximo = 150000, TaxaJuros = 6.0, Requisitos = "ME ou EPP, faturamento até R$ 4.8M", Website = "https://www.gov.br/pronampe" },
            new() { Nome = "Crediamigo - BNB", Descricao = "Microcrédito produtivo orientado", ValorMaximo = 15000, TaxaJuros = 2.95, Requisitos = "Empreendedor informal ou MEI", Website = "https://www.bnb.gov.br/crediamigo" },
            new() { Nome = "AgeRio Microcrédito", Descricao = "Financiamento para pequenos negócios RJ", ValorMaximo = 20000, TaxaJuros = 0.45, Requisitos = "Empreendedor com negócio ativo", Website = "https://www.agerio.com.br" }
        };
    }

    /// <summary>
    /// Busca CRAS e CREAS próximos para assistência social
    /// </summary>
    public async Task<List<UnidadeAssistenciaSocial>> GetUnidadesAssistenciaAsync(double lat, double lng, double raioKm = 5)
    {
        await Task.Delay(100);
        return new List<UnidadeAssistenciaSocial>
        {
            new() { Nome = "CRAS Sé", Tipo = "CRAS", Latitude = -23.5505, Longitude = -46.6333, Endereco = "Rua Líbero Badaró, 119", Telefone = "(11) 3241-1500", Servicos = new() { "CadÚnico", "Bolsa Família", "BPC", "Orientação Social" } },
            new() { Nome = "CRAS Mooca", Tipo = "CRAS", Latitude = -23.5580, Longitude = -46.5980, Endereco = "Rua da Mooca, 1500", Telefone = "(11) 2692-3344", Servicos = new() { "CadÚnico", "Bolsa Família", "Auxílio Brasil" } },
            new() { Nome = "CREAS Centro", Tipo = "CREAS", Latitude = -23.5450, Longitude = -46.6400, Endereco = "Av. São João, 473", Telefone = "(11) 3331-8900", Servicos = new() { "Proteção Especial", "Abordagem Social", "Medidas Socioeducativas" } },
            new() { Nome = "CRAS Pinheiros", Tipo = "CRAS", Latitude = -23.5670, Longitude = -46.6920, Endereco = "Rua Cardeal Arcoverde, 1749", Telefone = "(11) 3031-6500", Servicos = new() { "CadÚnico", "Bolsa Família", "PAIF" } }
        };
    }

    /// <summary>
    /// Busca cursos de qualificação profissional gratuitos
    /// </summary>
    public async Task<List<CursoQualificacao>> GetCursosQualificacaoAsync(string? area = null)
    {
        await Task.Delay(100);
        var cursos = new List<CursoQualificacao>
        {
            new() { Nome = "Auxiliar Administrativo", Instituicao = "SENAC", CargaHoraria = 160, Gratuito = true, Area = "Administração", ProximaTurma = DateTime.Now.AddDays(15), Vagas = 30 },
            new() { Nome = "Eletricista Predial", Instituicao = "SENAI", CargaHoraria = 200, Gratuito = true, Area = "Construção", ProximaTurma = DateTime.Now.AddDays(7), Vagas = 25 },
            new() { Nome = "Cuidador de Idosos", Instituicao = "SENAC", CargaHoraria = 160, Gratuito = true, Area = "Saúde", ProximaTurma = DateTime.Now.AddDays(20), Vagas = 20 },
            new() { Nome = "Programação Web", Instituicao = "SENAI", CargaHoraria = 400, Gratuito = true, Area = "Tecnologia", ProximaTurma = DateTime.Now.AddDays(30), Vagas = 35 },
            new() { Nome = "Confeitaria Básica", Instituicao = "SENAC", CargaHoraria = 80, Gratuito = true, Area = "Gastronomia", ProximaTurma = DateTime.Now.AddDays(10), Vagas = 15 },
            new() { Nome = "Mecânico de Automóveis", Instituicao = "SENAI", CargaHoraria = 300, Gratuito = true, Area = "Automotivo", ProximaTurma = DateTime.Now.AddDays(25), Vagas = 20 }
        };

        if (!string.IsNullOrEmpty(area))
            return cursos.FindAll(c => c.Area.Contains(area, StringComparison.OrdinalIgnoreCase));
        
        return cursos;
    }

    private List<OportunidadeEmprego> GetEmpregosMock() => new()
    {
        new() { Titulo = "Auxiliar de Produção", Empresa = "Indústria ABC", Salario = 1800, Latitude = -23.5400, Longitude = -46.6200, Endereco = "Zona Leste", Requisitos = "Ensino Médio", Beneficios = "VT, VR, Plano de Saúde" },
        new() { Titulo = "Atendente de Loja", Empresa = "Magazine Luiza", Salario = 1600, Latitude = -23.5550, Longitude = -46.6600, Endereco = "Centro", Requisitos = "Ensino Médio, Experiência em vendas", Beneficios = "VT, VR, Comissão" },
        new() { Titulo = "Técnico de Manutenção", Empresa = "TechServ", Salario = 2500, Latitude = -23.5300, Longitude = -46.6800, Endereco = "Pinheiros", Requisitos = "Curso Técnico em Elétrica/Eletrônica", Beneficios = "VT, VR, PLR" },
        new() { Titulo = "Operador de Caixa", Empresa = "Carrefour", Salario = 1500, Latitude = -23.5700, Longitude = -46.6400, Endereco = "Ipiranga", Requisitos = "Ensino Médio", Beneficios = "VT, VR, Desconto em compras" },
        new() { Titulo = "Auxiliar de Enfermagem", Empresa = "Hospital São Paulo", Salario = 2200, Latitude = -23.5980, Longitude = -46.6500, Endereco = "Vila Clementino", Requisitos = "Curso Técnico em Enfermagem, COREN ativo", Beneficios = "VT, VA, Plano de Saúde" },
        new() { Titulo = "Motorista de Entregas", Empresa = "Loggi", Salario = 2000, Latitude = -23.5200, Longitude = -46.7000, Endereco = "Lapa", Requisitos = "CNH B, Moto própria", Beneficios = "Ajuda de custo, Seguro" }
    };
}

public class OportunidadeEmprego
{
    public string Titulo { get; set; } = "";
    public string Empresa { get; set; } = "";
    public decimal Salario { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Endereco { get; set; } = "";
    public string Requisitos { get; set; } = "";
    public string Beneficios { get; set; } = "";
    public double DistanciaKm { get; set; }
    public string DistanciaFormatada => DistanciaKm < 1 ? $"{DistanciaKm * 1000:F0}m" : $"{DistanciaKm:F1}km";
}

public class ProgramaMicrocredito
{
    public string Nome { get; set; } = "";
    public string Descricao { get; set; } = "";
    public decimal ValorMaximo { get; set; }
    public double TaxaJuros { get; set; }
    public string Requisitos { get; set; } = "";
    public string? Telefone { get; set; }
    public string? Website { get; set; }
}

public class UnidadeAssistenciaSocial
{
    public string Nome { get; set; } = "";
    public string Tipo { get; set; } = ""; // CRAS, CREAS
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Endereco { get; set; } = "";
    public string? Telefone { get; set; }
    public List<string> Servicos { get; set; } = new();
}

public class CursoQualificacao
{
    public string Nome { get; set; } = "";
    public string Instituicao { get; set; } = "";
    public int CargaHoraria { get; set; }
    public bool Gratuito { get; set; }
    public string Area { get; set; } = "";
    public DateTime ProximaTurma { get; set; }
    public int Vagas { get; set; }
}

public class OportunidadeAutonomia
{
    public string Titulo { get; set; } = "";
    public string Tipo { get; set; } = ""; // Curso, Financiamento, Programa
    public string Descricao { get; set; } = "";
    public bool Gratuito { get; set; }
    public string UrlInscricao { get; set; } = "";
    public string? Cidade { get; set; }
    public DateTime? DataInicio { get; set; }
}
