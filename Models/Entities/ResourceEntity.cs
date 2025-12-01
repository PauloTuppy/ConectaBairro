using SQLite;
using System;

namespace ConectaBairro.Models.Entities;

public enum ResourceType
{
    School,
    Library,
    TrainingCenter,
    CommunityCenter,
    HealthClinic,
    Other
}

public class ResourceEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public ResourceType Type { get; set; }
    public string Address { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ContactInfo { get; set; } = ""; // Phone, Email, Website
}
