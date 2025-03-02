using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DTOs_BE.UserDTOs;
using Microsoft.Extensions.Configuration;
using Repositories_BE.Interfaces;
using Repositories_BE.Utils;
using Microsoft.AspNetCore.Identity;
using DataObjects_BE.Entities;
using Services_BE.Interfaces;

namespace Services_BE.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentTime _timeService;
        private readonly PasswordHasher<string> _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(IConfiguration configuration, IUserRepository userRepository, ICurrentTime timeService,
            IEmailService emailService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _timeService = timeService;
            _emailService = emailService;
            _passwordHasher = new PasswordHasher<string>();

        }

        public async Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel user)
        {
            /*var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];
            var adminRole = _configuration["Roles:Admin"];
*/
            List<Claim> claims = new List<Claim>();

            // Kiểm tra nếu là Admin
            /* if (user.Email == adminEmail && user.Password == adminPassword)
             {
                 claims.Add(new Claim(ClaimTypes.Name, "admin"));
                 claims.Add(new Claim(ClaimTypes.Email, adminEmail));
                 claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                 claims.Add(new Claim(ClaimTypes.Role, adminRole));

                 var adminToken = GenerateJWTToken.CreateToken(claims, _configuration, _timeService.GetCurrentTime());

                 return new ResponseLoginModel
                 {
                     Status = true,
                     Message = "Login successfully as Admin",
                     JWT = new JwtSecurityTokenHandler().WriteToken(adminToken),
                     Expired = adminToken.ValidTo,
                     JWTRefreshToken = null,
                     UserId = 0
                 };
             }*/

            // Kiểm tra xem user có tồn tại không
            var userExist = await _userRepository.GetByEmail(user.Email);
            if (userExist == null)
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "User does not exist."
                };
            }
            //Check OTP
            if (!userExist.IsActive)
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Your account has been deactivated. Please contact support."
                };
            }

            // So sánh mật khẩu đã băm
            var passwordVerification =
                _passwordHasher.VerifyHashedPassword(user.Email, userExist.Password, user.Password);
            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Invalid login attempt. Please check your password."
                };
            }

            // Tạo claims cho user
            claims.Add(new Claim(ClaimTypes.Name, userExist.AccountId.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, userExist.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            // Thêm role dựa vào AccountRole
            string role = userExist.Role switch
            {
                0 => "Manager",
                1 => "User",
                _ => "Doctor" // Mặc định user role = 1
            };
            claims.Add(new Claim("role", role));
            //claims.Add(new Claim(ClaimTypes.Role, role));

            // Tạo và lưu Refresh Token
            var refreshToken = TokenTools.GenerateRefreshToken();
            if (!int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays))
            {
                refreshTokenValidityInDays = 7; // Default to 7 days if config is invalid
            }

            // Tạo JWT Token
            var userToken = GenerateJWTToken.CreateToken(claims, _configuration, _timeService.GetCurrentTime());

            return new ResponseLoginModel
            {
                Status = true,
                Message = "Login successfully",
                JWT = new JwtSecurityTokenHandler().WriteToken(userToken),
                Expired = userToken.ValidTo,
                JWTRefreshToken = refreshToken,
                UserId = userExist.AccountId,
                FirstName = userExist.FirstName,
                LastName = userExist.LastName
            };
        }

        public async Task<RegisterResponseModel> Register(UserRegisterModel registerModel)
        {
            if (await _userRepository.CheckEmailExists(registerModel.Email))
            {
                return new RegisterResponseModel { Success = false, Message = "Email đã được sử dụng" };
            }

            if (!IsValidEmail(registerModel.Email))
            {
                return new RegisterResponseModel { Success = false, Message = "Email không hợp lệ" };
            }

            if (string.IsNullOrWhiteSpace(registerModel.Password) || registerModel.Password.Length < 6)
            {
                return new RegisterResponseModel { Success = false, Message = "Mật khẩu phải có ít nhất 6 ký tự" };
            }

            var account = new Account
            {
                AccountId = Guid.NewGuid(),
                UserName = registerModel.Email,
                Email = registerModel.Email,
                Password = _passwordHasher.HashPassword(registerModel.Email, registerModel.Password),
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                PhoneNumber = registerModel.PhoneNumber ?? "",
                Address = registerModel.Address ?? "",
                ImageUrl = "abc",
                Role = 1,
                DateCreateAt = DateTime.UtcNow,
                Otp = null,
                OtpCreatedAt = null,
                IsActive = true,
                DateUpdateAt = DateTime.UtcNow
            };
            var createAccount = await _userRepository.CreateAccount(account);


            if (createAccount == null)
            {
                return new RegisterResponseModel { Success = false, Message = "Tạo tài khoản thất bại" };
            }
            
            if (account.Role == 1)
            {
                var parent = new Parent
                {
                    ParentId = Guid.NewGuid(),
                    AccountId = account.AccountId
                };
                await _userRepository.CreateParent(parent);  // Thêm vào database
            }
            string otp = new Random().Next(100000, 999999).ToString();
            //DateTime otpCreatedAt = DateTime.UtcNow;
            await _userRepository.SaveOtp(registerModel.Email, otp);

            string emailBody = $"Mã OTP của bạn là: <b>{otp}</b>. Vui lòng nhập mã này để kích hoạt tài khoản.";
            try
            {
                await _emailService.SendVerifymailAsync(registerModel.Email, "Xác nhận đăng ký", emailBody);
            }
            catch (Exception ex)
            {
                return new RegisterResponseModel { Success = false, Message = $"Lỗi gửi email: {ex.Message}" };
            }



            return new RegisterResponseModel
            {
                Success = true,
                Message = "Đăng kí thành công. Vui lòng kiểm tra email để nhập mã OTP.",
                AccountId = account.AccountId
            };
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        /*public async Task<bool> VerifyOtpAsync(VerifyOtpModel model)
        {
            return await _userRepository.VerifyOtpAsync(model.Email, model.Otp);
        }*/

        public async Task<bool> VerifyOtpAsync(VerifyOtpModel model)
        {
            var otpInfo = await _userRepository.GetOtpInfoAsync(model.Email);
            if (otpInfo == null) return false;
            if (otpInfo.OtpCreatedAt == null)
            {
                return false;
            }

            var elapsedTime = DateTime.UtcNow - otpInfo.OtpCreatedAt.Value;

            if (elapsedTime.TotalMinutes > 5)
            {
                return false;
            }

            return otpInfo.OtpCode == model.Otp;
        }


        public async Task<bool> ActivateAccountAsync(VerifyOtpModel model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null) return false;

            bool isOtpValid = await _userRepository.VerifyOtpAsync(model.Email, model.Otp);
            if (!isOtpValid) return false;

            user.IsActive = true;
            await _userRepository.UpdateUserAsync(user);

            return true;
        }

        public async Task<RegisterResponseModel> ResendOtp(string email)
        {

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return new RegisterResponseModel { Success = false, Message = "Email không tồn tại" };
            }

            var otpInfo = await _userRepository.GetOtpInfoAsync(email);

            if (otpInfo == null || otpInfo.OtpCreatedAt == null || (DateTime.UtcNow - otpInfo.OtpCreatedAt.Value).TotalMinutes > 5)
            {
                string otp = new Random().Next(100000, 999999).ToString();
                await _userRepository.SaveOtp(email, otp);

                string emailBody = $"Mã OTP của bạn là: <b>{otp}</b>. Vui lòng nhập mã này để kích hoạt tài khoản.";
                try
                {
                    await _emailService.SendVerifymailAsync(email, "Xác nhận đăng ký", emailBody);
                }
                catch (Exception ex)
                {
                    return new RegisterResponseModel { Success = false, Message = $"Lỗi gửi email: {ex.Message}" };
                }

                return new RegisterResponseModel
                {
                    Success = true,
                    Message = "OTP mới đã được gửi đến email của bạn."
                };
            }

            return new RegisterResponseModel { Success = false, Message = "OTP chưa hết hạn" };
        }
        public async Task<bool> ChangeUserRoleAsync(Guid accountId, int newRole)
        {
            var user = await _userRepository.GetByIdAsync(accountId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (user.Role == 0)
            {
                throw new InvalidOperationException("Cannot change role of another Manager");
            }

            user.Role = newRole;
            user.DateUpdateAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return true;
        }




        //Register Account
        /*public async Task<RegisterResponseModel> Register(UserRegisterModel registerModel)
        {
            //Kiểm tra email đã tồn tại hay chưa
            if (await _userRepository.CheckEmailExists(registerModel.Email))
            {
                return new RegisterResponseModel
                {
                    Success = false,
                    Message = "Email đã được sử dụng"
                };
            }

            //Tạo account mới
            var account = new Account
            {
                AccountId = Guid.NewGuid(),
                UserName = registerModel.Email, // Sử dụng email làm username
                Email = registerModel.Email,
                Password = _passwordHasher.HashPassword(registerModel.Email, registerModel.Password),
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                PhoneNumber = registerModel.PhoneNumber ?? "",
                Address = registerModel.Address ?? "",
                ImageUrl = "",
                Role = 1, //Mặc định là user
                DateCreateAt = DateTime.UtcNow
            };

            //Lưu account vào database
            var createAccount = await _userRepository.CreateAccount(account);
            return new RegisterResponseModel
            {
                Success = true,
                Message = "Đăng kí thành công",
                AccountId = account.AccountId
            };
        }*/

        //Change Password
        public async Task<bool> ChangePasswordAsync(Guid accountId, ChangePasswordModel model)
        {
            //1. Check tính xác thực tài khoản
            var user = await _userRepository.GetAsync(u => u.AccountId == accountId);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại.");
            }

            //2. Check mật khẩu cũ có đúng hay không
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user.Email, user.Password, model.OldPassword);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                throw new Exception("Mật khẩu cũ không đúng.");
            }

            //3. Check mật khẩu mới phải có ít nhất 6 ký tự
            if (string.IsNullOrWhiteSpace(model.NewPassword) || model.NewPassword.Length < 6)
            {
                throw new Exception("Mật khẩu mới phải có ít nhất 6 ký tự.");
            }

            //4. Check confirmPassword có giống với newPassword không
            if (model.NewPassword != model.ConfirmPassword)
            {
                throw new Exception("Mật khẩu xác nhận không khớp với mật khẩu mới.");
            }

            //5. Lưu mật khẩu mới và băm mật khẩu
            user.Password = _passwordHasher.HashPassword(user.Email, model.NewPassword);
            await _userRepository.UpdateUserAsync(user);
            return true;

        }

        //Reset Password
        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                return false; // Mật khẩu mới và xác nhận mật khẩu không khớp
            }

            var user = await _userRepository.GetUserByResetPasswordTokenAsync(model.Token);
            if (user == null || user.Email != model.Email)
            {
                return false; // Token không hợp lệ hoặc email không khớp
            }

            // Cập nhật mật khẩu mới cho người dùng
            user.Password = _passwordHasher.HashPassword(user.Email, model.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiration = null;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        //Send Request Link
        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return false; // Người dùng không tồn tại
            }

            //var token = TokenTools.GenerateRefreshToken();
            // Lưu token vào cơ sở dữ liệu hoặc một nơi lưu trữ tạm thời với thời gian hết hạn
            //var resetLink = $"{_configuration["AppSettings:ClientUrl"]}/reset-password?token={token}&email={email}";
            //var emailBody = $"Please reset your password by clicking <a href='{resetLink}'>here</a>";

            var token = Guid.NewGuid().ToString(); // Sử dụng GUID làm mã thông báo
            var expiry = DateTime.UtcNow.AddHours(1); 
            await _userRepository.SaveResetPasswordTokenAsync(email, token, expiry);

            var resetLink = $"{_configuration["AppSettings:ClientUrl"]}/reset-password?token={token}&email={email}";
            var emailBody = $"Please reset your password by clicking <a href='{resetLink}'>here</a>";

            await _emailService.SendVerifymailAsync(email, "Reset Password", emailBody);

            return true;
        }

        



    }
}