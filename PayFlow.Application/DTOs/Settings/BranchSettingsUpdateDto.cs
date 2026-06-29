namespace PayFlow.Application.DTOs.Settings;

public class BranchSettingsUpdateDto
{
    public string SchoolName { get; set; } = string.Empty;
    public string? PrincipalName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Contact { get; set; }
}
