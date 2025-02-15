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
        Task<Account> GetByEmail(string email);


    }
}
