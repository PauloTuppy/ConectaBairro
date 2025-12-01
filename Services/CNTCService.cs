using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de integração com CNCT - Catálogo Nacional de Cursos Técnicos (MEC)
/// https://cnct.mec.gov.br/
/// </summary>
public class CNTCService
{
    private static CNTCService? _instance;
    public static CNTCService Instance => _instance ??= new CNTCService();

    private readonly HttpClient _httpClient = new();
    private const string BaseUrl = "https://cnct.mec.gov.br";

    private CNTCService() 
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "ConectaBairro/1.0");
    }

    /// <summary>
    /// Busca cursos técnicos por eixo tecnológico
    /// </summary>
    public async Task<List<CursoTecnico>> GetCursosPorEixoAsync(string eixo)
    {
        try
        {
            // Em produção, fazer scraping ou usar API oficial se disponível
            await Task.Delay(100);
            return GetCursosMock().FindAll(c => c.EixoTecnologico.Contains(eixo, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro CNCT: {ex.Message}");
            return new List<CursoTecnico>();
        }
    }

    /// <summary>
    /// Busca todos os cursos técnicos disponíveis
    /// </summary>
    public async Task<List<CursoTecnico>> GetAllCursosAsync()
    {
        await Task.Delay(100);
        return GetCursosMock();
    }

    /// <summary>
    /// Busca cursos por região/estado
    /// </summary>
    public async Task<List<CursoTecnico>> GetCursosPorRegiaoAsync(string estado, string? cidade = null)
    {
        var cursos = await GetAllCursosAsync();
        return cursos.FindAll(c => 
            c.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase) &&
            (cidade == null || c.Cidade.Contains(cidade, StringComparison.OrdinalIgnoreCase)));
    }

    /// <summary>
    /// Busca instituições que oferecem cursos técnicos
    /// </summary>
    public async Task<List<InstituicaoTecnica>> GetInstituicoesAsync(double lat, double lng, double raioKm = 50)
    {
        await Task.Delay(100);
        return new List<InstituicaoTecnica>
        {
            new() { Nome = "SENAI Vila Leopoldina", Tipo = "SENAI", Latitude = -23.5289, Longitude = -46.7195, Endereco = "Rua Jaguaré, 500", CursosDisponiveis = 15 },
            new() { Nome = "SENAC Consolação", Tipo = "SENAC", Latitude = -23.5505, Longitude = -46.6533, Endereco = "Rua Dr. Vila Nova, 228", CursosDisponiveis = 22 },
            new() { Nome = "ETEC Parque da Juventude", Tipo = "ETEC", Latitude = -23.5155, Longitude = -46.6270, Endereco = "Av. Cruzeiro do Sul, 2630", CursosDisponiveis = 18 },
            new() { Nome = "IFSP Campus São Paulo", Tipo = "IF", Latitude = -23.5290, Longitude = -46.6325, Endereco = "Rua Pedro Vicente, 625", CursosDisponiveis = 25 },
            new() { Nome = "FATEC Ipiranga", Tipo = "FATEC", Latitude = -23.5870, Longitude = -46.6080, Endereco = "Av. Nazaré, 2001", CursosDisponiveis = 12 }
        };
    }

    private List<CursoTecnico> GetCursosMock() => new()
    {
        // Eixo: Informação e Comunicação
        new() { Nome = "Técnico em Desenvolvimento de Sistemas", EixoTecnologico = "Informação e Comunicação", CargaHoraria = 1200, Estado = "SP", Cidade = "São Paulo", Modalidade = "Presencial", Gratuito = true },
        new() { Nome = "Técnico em Redes de Computadores", EixoTecnologico = "Informação e Comunicação", CargaHoraria = 1000, Estado = "SP", Cidade = "São Paulo", Modalidade = "Presencial", Gratuito = true },
        new() { Nome = "Técnico em Informática para Internet", EixoTecnologico = "Informação e Comunicação", CargaHoraria = 1000, Estado = "SP", Cidade = "Campinas", Modalidade = "EAD", Gratuito = true },
        
        // Eixo: Gestão e Negócios
        new() { Nome = "Técnico em Administração", EixoTecnologico = "Gestão e Negócios", CargaHoraria = 800, Estado = "SP", Cidade = "São Paulo", Modalidade = "Presencial", Gratuito = true },
        new() { Nome = "Técnico em Contabilidade", EixoTecnologico = "Gestão e Negócios", CargaHoraria = 800, Estado = "SP", Cidade = "Santos", Modalidade = "Presencial", Gratuito = false },
        new() { Nome = "Técnico em Logística", EixoTecnologico = "Gestão e Negócios", CargaHoraria = 800, Estado = "SP", Cidade = "São Paulo", Modalidade = "Híbrido", Gratuito = true },
        
        // Eixo: Saúde
        new() { Nome = "Técnico em Enfermagem", EixoTecnologico = "Ambiente e Saúde", CargaHoraria = 1800, Estado = "SP", Cidade = "São Paulo", Modalidade = "Presencial", Gratuito = true },
        new() { Nome = "Técnico em Farmácia", EixoTecnologico = "Ambiente e Saúde", CargaHoraria = 1200, Estado = "SP", Cidade = "Ribeirão Preto", Modalidade = "Presencial", Gratuito = false },
        
        // Eixo: Indústria
        new() { Nome = "Técnico em Mecânica", EixoTecnologico = "Controle e Processos Industriais", CargaHoraria = 1200, Estado = "SP", Cidade = "São Bernardo", Modalidade = "Presencial", Gratuito = true },
        new() { Nome = "Técnico em Eletrotécnica", EixoTecnologico = "Controle e Processos Industriais", CargaHoraria = 1200, Estado = "SP", Cidade = "São Paulo", Modalidade = "Presencial", Gratuito = true },
        new() { Nome = "Técnico em Automação Industrial", EixoTecnologico = "Controle e Processos Industriais", CargaHoraria = 1200, Estado = "SP", Cidade = "Guarulhos", Modalidade = "Presencial", Gratuito = true }
    };
}

public class CursoTecnico
{
    public string Nome { get; set; } = "";
    public string EixoTecnologico { get; set; } = "";
    public int CargaHoraria { get; set; }
    public string Estado { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Modalidade { get; set; } = ""; // Presencial, EAD, Híbrido
    public bool Gratuito { get; set; }
    public string? UrlInscricao { get; set; }
}

public class InstituicaoTecnica
{
    public string Nome { get; set; } = "";
    public string Tipo { get; set; } = ""; // SENAI, SENAC, ETEC, IF, FATEC
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Endereco { get; set; } = "";
    public int CursosDisponiveis { get; set; }
    public string? Telefone { get; set; }
    public string? Website { get; set; }
}
