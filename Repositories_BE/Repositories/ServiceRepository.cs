﻿using DataObjects_BE;
using DataObjects_BE.Entities;
using Repositories_BE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories_BE.Repositories
{
    public class ServiceRepository:GenericRepository<Service>, IServiceRepositoy
    {
        private readonly SWP391G3DbContext _context;
        public ServiceRepository(SWP391G3DbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Service?> GetServiceByIdAsync(int serviceId)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == serviceId);
        }

        public async Task<Service> GetByIdAsync(int serviceId)
        {
            return await _context.Services.FindAsync(serviceId);
        }
    }
}
