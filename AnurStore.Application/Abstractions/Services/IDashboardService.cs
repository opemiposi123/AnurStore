using AnurStore.Application.DTOs;

namespace AnurStore.Application.Abstractions.Services
{
    public interface  IDashboardService
    {
        Task<DashboardCountDto> GetDashboardDataAsync();
    }
}
