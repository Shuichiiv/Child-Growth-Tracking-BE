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
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Repositories_BE.Interfaces;
using Repositories_BE.Utils;
using DTOs_BE.UserDTOs;
using Microsoft.EntityFrameworkCore;


namespace Repositories_BE.Repositories
{
    public class UserRepository : GenericRepository<Account>, IUserRepository
    {
        private readonly SWP391G3DbContext _context;

        public UserRepository(SWP391G3DbContext context)
            : base(context)
        {
            _context = context;
        }
        public async Task<Account> GetByEmail(string email)
        {
            //Tìm user bằng email
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email);
        }
        
        public async Task<bool> CheckEmailExists(string email)
        {
            //kiểm tra email đã tồn tại
            return await _context.Accounts.AnyAsync(x => x.Email == email);
        }

        public async Task<Account> CreateAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
            if (await CheckEmailExists(account.Email))
                throw new InvalidOperationException("Email already exists");
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }
        public async Task SaveOtp(string email, string otp)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                user.Otp = otp;
                user.OtpCreatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<bool> CreateParent(Parent parent)
        {
            _context.Parents.Add(parent);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string> GetOtpAsync(string email)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            return user?.Otp;
        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            // 5 min
            if (user.Otp == otp && user.OtpCreatedAt > DateTime.UtcNow.AddMinutes(-5))
            {
                user.Otp = null;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        
        public async Task UpdateUserAsync(Account user)
        {
            _context.Accounts.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Account> GetUserByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }
        
        public async Task SaveOtp(string email, string otp, DateTime otpCreatedAt)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                user.Otp = otp;
                user.OtpCreatedAt = otpCreatedAt;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<OtpInfo> GetOtpInfoAsync(string email)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            return new OtpInfo 
            { 
                OtpCode = user.Otp, 
                CreatedAt = user.OtpCreatedAt
            };
        }
        
        public async Task<Account> GetByIdAsync(Guid accountId)
        {
            return await _context.Accounts.FindAsync(accountId);
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task SaveResetPasswordTokenAsync(string email, string token, DateTime expiry)
        {
            var user = await GetUserByEmailAsync(email);
            if (user != null)
            {
                user.ResetPasswordToken = token;
                user.ResetPasswordTokenExpiration = expiry;
                await UpdateUserAsync(user);
            }
        }

        public async Task<Account> GetUserByResetPasswordTokenAsync(string token)
        {
            return await _context.Set<Account>().FirstOrDefaultAsync(u => u.ResetPasswordToken == token && u.ResetPasswordTokenExpiration > DateTime.UtcNow);
        }

    }
}
