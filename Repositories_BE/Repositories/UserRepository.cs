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
            // Kiểm tra dữ liệu đầu vào
            if (account == null)
                throw new ArgumentNullException(nameof(account), "Account cannot be null");

            // Kiểm tra email đã tồn tại
            if (await CheckEmailExists(account.Email))
                throw new InvalidOperationException("Email already exists");

            //Thêm account mới vào database
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

    }
}
