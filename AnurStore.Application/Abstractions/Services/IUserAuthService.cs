using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IUserAuthService
    {
        Task<Status> LoginAsync(LoginRequestModel model);
        Task LogoutAsync();
        Task<Status> ChangePasswordAsync(ChangePasswordRequestModel model, string username);
    }
}
