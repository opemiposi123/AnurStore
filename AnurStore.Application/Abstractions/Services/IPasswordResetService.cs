using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IPasswordResetService
    {
        Task<BaseResponse<bool>> CreateNewPassWord(CreateNewPassWord request);
        Task<BaseResponse<bool>> ValidateResetCodeAsync(ValidateResetCode request);
    }
}
