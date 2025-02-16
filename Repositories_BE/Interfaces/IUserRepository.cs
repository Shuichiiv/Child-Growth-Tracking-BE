using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;

namespace Repositories_BE.Interfaces
{
    public interface IUserRepository: IGenericRepository<Account>
    {
        Task<Account> GetByEmail(string email); //Đăng nhập bằng email
        Task<bool> CheckEmailExists(string email); //Kiểm tra email đã tồn tại trong hệ thống chưa
        Task<Account> CreateAccount(Account account); //Tạo tài khoản mới trong database

    }
}
