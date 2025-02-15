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

namespace Services_BE.Services
{
    public class UserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentTime _timeService;
        private readonly PasswordHasher<string> _passwordHasher;



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
                _ => "Doctor" // Mặc định user role = 3
            };
            claims.Add(new Claim(ClaimTypes.Role, role));

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
    }
}
