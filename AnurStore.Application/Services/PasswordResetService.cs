using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AnurStore.Application.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PasswordReset> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;
        private readonly IPasswordResetRepository _passwordResetRepository;

        public PasswordResetService(ILogger<PasswordReset> logger,
          UserManager<User> userManager, IEmailService emailSender,
          IPasswordResetRepository passwordResetRepository,
          IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
           _passwordResetRepository = passwordResetRepository;
        }
        public async Task<BaseResponse<bool>> CreateNewPassWord(CreateNewPassWord request)
        {
            try
            {
                _logger.LogInformation("Start Create Password Request");

                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    _logger.LogInformation($"User with the {request.Email} does not exist");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = $"User with the {request.Email} does not exist"
                    };
                }


                // Generate the reset code
                string resetCode = GenerateRandomCode(8);
                var userPrincipal = _httpContextAccessor.HttpContext?.User;
                if (userPrincipal == null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "User not authenticated"
                    };
                }

                var username = await _userManager.GetUserAsync(userPrincipal);
                if (user == null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "User not found"
                    };
                }
                var passwordResetRequest = new PasswordReset
                {
                    Email = request.Email,
                    ResetCode = resetCode,
                    UserId = user.Id,
                    RequestedAt = DateTime.UtcNow,
                    CreatedOn = DateTime.Now,
                    IsUsed = false,

                };

                // Save to the database
             await  _passwordResetRepository.AddAsync(passwordResetRequest);

                // Prepare and send the email
                var mailRequest = new MailRequests
                {
                    Body = $"<p>Hello {user.FirstName},</p><p>Your password reset code is: <strong>{resetCode}</strong></p>",
                    Title = "Password Reset Code",
                    ToEmail = user.Email,
                    HtmlContent = $"<p>Hello {user.FirstName},</p><p>Your password reset code is: <strong>{resetCode}</strong></p>"
                };

                var emailResponse = await _emailSender.SendEmailAsync(new MailReceiverDto { Email = user.Email, Name = user.FirstName + " " + user.LastName }, mailRequest);

                if (!emailResponse)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Failed to send reset code"
                    };
                }

                _logger.LogInformation("Code Generated Successfully");
                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = "Code Generated Successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the reset code");
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = "Code Creation failed"
                };
            }
        }


        public async Task<BaseResponse<bool>> ValidateResetCodeAsync(ValidateResetCode request)
        {
            try
            {
                var resetRequest = await _passwordResetRepository.GetByResetCodeAsync(request.ResetCode);

                if (resetRequest == null || (DateTime.UtcNow - resetRequest.RequestedAt).TotalMinutes > 5)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "The code you entered has expired or is invalid."
                    };
                }

                var user = await _userManager.FindByIdAsync(resetRequest.UserId);
                if (user == null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "User not found"
                    };
                }

                var result = await _userManager.ResetPasswordAsync(user, await _userManager.GeneratePasswordResetTokenAsync(user), request.NewPassword);

                if (result.Succeeded)
                {
                    resetRequest.IsUsed = true;
                    resetRequest.UsedAt = DateTime.UtcNow;
                    await _passwordResetRepository.UpdateAsync(resetRequest);

                    return new BaseResponse<bool>
                    {
                        Status = true,
                        Message = "Password reset successful"
                    };
                }

                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = "Failed to reset password"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating the reset code");
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = "An error occurred"
                };
            }
        }



        private string GenerateRandomCode(int length)
        {
            var random = new Random();
            var randomNumber = random.Next(100000, 1000000);

            return randomNumber.ToString();
        }

    }
}
