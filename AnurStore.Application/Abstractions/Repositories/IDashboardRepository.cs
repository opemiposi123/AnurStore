using AnurStore.Application.DTOs;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardCountDto> DashBoardDataAsync();
    }
}
