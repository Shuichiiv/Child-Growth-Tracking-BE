using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs_BE.UserDTOs;

namespace Services_BE.Interfaces
{
    public interface IUserService
    {
        Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel user);
    }
}
