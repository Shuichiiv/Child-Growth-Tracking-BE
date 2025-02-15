using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Repositories_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Repositories_BE.Utils;

namespace Repositories_BE.Common
{
    public class ClaimsService : IClaimsService
    {
        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            // todo implementation to get the current userId
            var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var extractedId = AuthenTools.GetCurrentUserId(identity);
            GetCurrentUserId = string.IsNullOrEmpty(extractedId) ? -1 : int.Parse(extractedId);
            IpAddress = httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();
        }

        public int GetCurrentUserId { get; }

        public string? IpAddress { get; }
    }
}
