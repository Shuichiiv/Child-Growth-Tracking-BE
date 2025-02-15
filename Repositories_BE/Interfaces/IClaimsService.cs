using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories_BE.Interfaces
{
    public interface IClaimsService
    {
        public int GetCurrentUserId { get; }

        public string? IpAddress { get; }

    }
}
