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
    public class UserService: IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentTime _timeService;
        private readonly PasswordHasher<string> _passwordHasher;

        public UserService(IConfiguration configuration, IUserRepository userRepository, ICurrentTime timeService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _timeService = timeService;
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

            // So sánh mật khẩu đã băm
            var passwordVerification = _passwordHasher.VerifyHashedPassword(user.Email, userExist.Password, user.Password);
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
                refreshTokenValidityInDays = 7;  // Default to 7 days if config is invalid
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
                UserId = userExist.AccountId
            };
        }


        //Register Account
        public async Task<RegisterResponseModel> Register(UserRegisterModel registerModel)
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
        }
    }
}
