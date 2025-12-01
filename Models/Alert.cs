using System;

namespace ConectaBairro.Models;

public enum AlertType
{
    Info,
    Warning,
    Emergency,
    Opportunity
}

public record Alert
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = "";
    public string Message { get; init; } = "";
    public AlertType Type { get; init; }
    public string Location { get; init; } = ""; // Bairro, Rua, etc.
    public DateTime Timestamp { get; init; } = DateTime.Now;
    public bool IsActive { get; init; } = true;
}