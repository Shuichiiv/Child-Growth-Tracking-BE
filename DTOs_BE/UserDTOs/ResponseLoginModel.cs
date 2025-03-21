﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.UserDTOs
{
    public class ResponseLoginModel
    {
        public bool Status { get; set; } = false;

        public string Message { get; set; } = "";

        public string JWT { get; set; } = "";

        public DateTime? Expired { get; set; }

        public string? JWTRefreshToken { get; set; } = "";

        public Guid? UserId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
