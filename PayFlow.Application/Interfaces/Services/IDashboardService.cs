using PayFlow.Application.DTOs.Dashboard;

namespace PayFlow.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<AdminDashboardDto> GetAdminDashboardAsync();
}
