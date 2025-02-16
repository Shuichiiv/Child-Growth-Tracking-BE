using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.UserDTOs
{
    public class RegisterResponseModel
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public Guid? AccountId { get; set; }

    }
}
