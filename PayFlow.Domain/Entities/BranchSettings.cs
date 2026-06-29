namespace PayFlow.Domain.Entities;

public class BranchSettings
{
    public int Id { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string? PrincipalName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Contact { get; set; }
    public string? LogoPath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
