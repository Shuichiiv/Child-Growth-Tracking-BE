using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs_BE.UserDTOs;
using Microsoft.AspNetCore.Identity;

namespace Services_BE.Interfaces
{
    public interface IUserService
    {
        Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel user);
        Task<RegisterResponseModel> Register(UserRegisterModel registerModel);
        Task<bool> VerifyOtpAsync(VerifyOtpModel otpModel);
        Task<bool> ActivateAccountAsync(VerifyOtpModel otpModel);
        Task<RegisterResponseModel> ResendOtp(string email);
        Task<bool> ChangePasswordAsync(Guid accountId, ChangePasswordModel model);


    }
}
