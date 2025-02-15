using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE;
using DataObjects_BE.Entities;
//using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories_BE.Interfaces;
using Repositories_BE.Utils;
//using DTOs_BE.UserDTOs;

namespace Repositories_BE.Repositories
{
    //public class UserRepository : GenericRepository<Account>, IUserRepository
    //{
    //    private readonly SWP391G3DbContext _context;
    //    private readonly IConfiguration _configuration;
    //    private readonly ISystemAccountDAO _systemAccountDAO;
    //    private readonly ITimeService _timeService;
    //    private readonly PasswordHasher<string> _passwordHasher;

    //    public UserRepository(SWP391G3DbContext context, IConfiguration configuration, ISystemAccountDAO systemAccountDAO, ITimeService timeService)
    //        : base(context)
    //    {
    //        _context = context;
    //        _configuration = configuration;
    //        _systemAccountDAO = systemAccountDAO;
    //        _timeService = timeService;
    //        _passwordHasher = new PasswordHasher<string>();
    //    }

    //    public async Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel user)
    //    {
    //        var adminEmail = _configuration["AdminAccount:Email"];
    //        var adminPassword = _configuration["AdminAccount:Password"];
    //        var adminRole = _configuration["Roles:Admin"];

    //        List<Claim> claims = new List<Claim>();

    //        // Kiểm tra nếu là Admin
    //        if (user.Email == adminEmail && user.Password == adminPassword)
    //        {
    //            claims.Add(new Claim(ClaimTypes.Name, "admin"));
    //            claims.Add(new Claim(ClaimTypes.Email, adminEmail));
    //            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
    //            claims.Add(new Claim(ClaimTypes.Role, adminRole));

    //            var adminToken = GenerateJWTToken.CreateToken(claims, _configuration, _timeService.GetCurrentTime());

    //            return new ResponseLoginModel
    //            {
    //                Status = true,
    //                Message = "Login successfully as Admin",
    //                JWT = new JwtSecurityTokenHandler().WriteToken(adminToken),
    //                Expired = adminToken.ValidTo,
    //                JWTRefreshToken = null,
    //                UserId = 0
    //            };
    //        }

    //        // Kiểm tra xem user có tồn tại không
    //        var userExist = await _systemAccountDAO.GetByEmail(user.Email);
    //        if (userExist == null)
    //        {
    //            return new ResponseLoginModel
    //            {
    //                Status = false,
    //                Message = "User does not exist."
    //            };
    //        }

    //        // So sánh mật khẩu đã băm
    //        var passwordVerification = _passwordHasher.VerifyHashedPassword(user.Email, userExist.AccountPassword, user.Password);
    //        if (passwordVerification == PasswordVerificationResult.Failed)
    //        {
    //            return new ResponseLoginModel
    //            {
    //                Status = false,
    //                Message = "Invalid login attempt. Please check your password."
    //            };
    //        }

    //        // Tạo claims cho user
    //        claims.Add(new Claim(ClaimTypes.Name, userExist.AccountId.ToString()));
    //        claims.Add(new Claim(ClaimTypes.Email, userExist.AccountEmail));
    //        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

    //        // Thêm role dựa vào AccountRole
    //        string role = userExist.AccountRole switch
    //        {
    //            "Staff" => "1",
    //            "Lecturer" => "2",
    //            _ => "3" // Mặc định user role = 3
    //        };
    //        claims.Add(new Claim(ClaimTypes.Role, role));

    //        // Tạo và lưu Refresh Token
    //        var refreshToken = TokenTools.GenerateRefreshToken();
    //        userExist.RefreshToken = refreshToken;
    //        userExist.RefreshTokenExpiryTime = _timeService.GetCurrentTime().AddDays(7); // Refresh Token có hiệu lực 7 ngày
    //        _systemAccountDAO.Update(userExist);
    //        await _systemAccountDAO.SaveAsync();

    //        // Tạo JWT Token
    //        var userToken = GenerateJWTToken.CreateToken(claims, _configuration, _timeService.GetCurrentTime());

    //        return new ResponseLoginModel
    //        {
    //            Status = true,
    //            Message = "Login successfully",
    //            JWT = new JwtSecurityTokenHandler().WriteToken(userToken),
    //            Expired = userToken.ValidTo,
    //            JWTRefreshToken = refreshToken,
    //            UserId = userExist.AccountId
    //        };
    //    }
    //}
}
