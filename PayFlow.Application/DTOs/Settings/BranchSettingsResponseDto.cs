namespace PayFlow.Application.DTOs.Settings;

public class BranchSettingsResponseDto
{
    public int Id { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string? PrincipalName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Contact { get; set; }
    public string? LogoPath { get; set; }
    public string? LastLogin { get; set; }
    public string? Username { get; set; }
}