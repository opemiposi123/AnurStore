using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<BaseResponse<string>> AddUser(CreateUserRequest request);
        Task<List<UserDto>> LoadAllUser(); 
        Task<UserDto> LoadUserDetail(string id); 
        Task<BaseResponse<string>> UpdateUser(string id, UserDto updateMemberDto);
        Task<BaseResponse<bool>> DeleteUser(string id);
    }
}
