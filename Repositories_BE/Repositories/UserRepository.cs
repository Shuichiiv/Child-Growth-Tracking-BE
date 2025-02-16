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
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email);
        }

    }
}
