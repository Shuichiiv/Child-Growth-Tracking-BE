﻿using DataObjects_BE;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Common;
using Repositories_BE.Interfaces;
using Repositories_BE.Repositories;
using Services_BE.Interfaces;
using Services_BE.Mapper;
using Services_BE.Services;
using System.Diagnostics;
using WebAPI_BE.MiddleWare;


namespace WebAPI_BE.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServicesInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SWP391G3DbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DataObjects_BE"))); // Chạy Migration vào DataObjects_BE


            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddTransient<PerformanceTimeMiddleware>();
            services.AddScoped<UserStatusMiddleware>();


            //Injection currenttime and claims
            services.AddScoped<ICurrentTime, CurrentTime>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddSingleton<Stopwatch>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperConfigProfile).Assembly);

            //Injection repositories of project
            services.AddScoped<IServiceRepositoy, ServiceRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            

            //Injection services of project
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
