using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;

namespace Services_BE.Interfaces
{
    public interface IUserService
    {
        Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel user);
        Task<RegisterResponseModel> Register(UserRegisterModel registerModel);
        Task<bool> VerifyOtpAsync(VerifyOtpModel otpModel);
        Task<bool> ActivateAccountAsync(VerifyOtpModel otpModel);
        Task<RegisterResponseModel> ResendOtp(string email);
        
        Task<bool> ChangeUserRoleAsync(Guid accountId, int newRole);
        Task<bool> ChangePasswordAsync(Guid accountId, ChangePasswordModel model);
        //Task<bool> ChangePasswordByOtpAsync(ChangePasswordByOtpModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
        Task<bool> RequestPasswordResetAsync(string email);

    }
}
