using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConectaBairro.Services;

/// <summary>
/// Serviço de certificados digitais
/// Emissão, validação e compartilhamento
/// </summary>
public class CertificateService
{
    private static CertificateService? _instance;
    public static CertificateService Instance => _instance ??= new CertificateService();

    private readonly List<Certificate> _certificates = new();
    private const string BaseValidationUrl = "https://conectabairro.com/verify/";

    private CertificateService() { }

    public async Task<Certificate> IssueCertificateAsync(string userId, string courseName, string courseProvider, int hoursCompleted)
    {
        var certificate = new Certificate
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CourseName = courseName,
            CourseProvider = courseProvider,
            HoursCompleted = hoursCompleted,
            VerificationCode = GenerateVerificationCode(),
            IssuedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddYears(2),
            Status = CertificateStatus.Active
        };

        certificate.ValidationUrl = $"{BaseValidationUrl}{certificate.VerificationCode}";
        certificate.QRCodeData = GenerateQRCodeData(certificate);

        _certificates.Add(certificate);
        await Task.CompletedTask;
        return certificate;
    }

    public async Task<Certificate?> ValidateCertificateAsync(string verificationCode)
    {
        await Task.Delay(50);
        var cert = _certificates.Find(c => c.VerificationCode == verificationCode);
        
        if (cert == null) return null;
        if (cert.ExpiresAt < DateTime.Now) cert.Status = CertificateStatus.Expired;
        
        return cert;
    }

    public async Task<List<Certificate>> GetUserCertificatesAsync(string userId)
    {
        await Task.Delay(50);
        return _certificates.FindAll(c => c.UserId == userId);
    }

    public async Task<bool> RevokeCertificateAsync(Guid certificateId, string reason)
    {
        var cert = _certificates.Find(c => c.Id == certificateId);
        if (cert != null)
        {
            cert.Status = CertificateStatus.Revoked;
            cert.RevokedReason = reason;
        }
        await Task.CompletedTask;
        return cert != null;
    }

    public string GetShareableLink(Certificate certificate) =>
        $"https://conectabairro.com/certificate/{certificate.Id}";

    private static string GenerateVerificationCode() =>
        $"{Guid.NewGuid().ToString("N")[..8].ToUpper()}{DateTime.Now:yyMMdd}";

    private static string GenerateQRCodeData(Certificate cert) =>
        $"CERT:{cert.VerificationCode}|{cert.CourseName}|{cert.IssuedAt:yyyy-MM-dd}";
}

public class Certificate
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = "";
    public string CourseName { get; set; } = "";
    public string CourseProvider { get; set; } = "";
    public int HoursCompleted { get; set; }
    public string VerificationCode { get; set; } = "";
    public string ValidationUrl { get; set; } = "";
    public string QRCodeData { get; set; } = "";
    public DateTime IssuedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public CertificateStatus Status { get; set; }
    public string? RevokedReason { get; set; }
    
    public bool IsValid => Status == CertificateStatus.Active && ExpiresAt > DateTime.Now;
}

public enum CertificateStatus { Active, Expired, Revoked }
