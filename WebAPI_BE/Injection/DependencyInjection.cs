using DataObjects_BE;
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

            services.AddHostedService<ServiceOrderBackgroundService>();



            //Injection currenttime and claims
            services.AddScoped<ICurrentTime, CurrentTime>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddSingleton<Stopwatch>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperConfigProfile).Assembly);

            //Injection repositories of project
            services.AddScoped<IServiceRepositoy, ServiceRepository>();
            services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IParentRepository, ParentRepository>();
            services.AddScoped<IChildRepository, ChildRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            //Injection services of project
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IVietQRService, VietQRService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceOrderService, ServiceOrderService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IChildService, ChildService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<PayOSService>();
            services.AddScoped<PaymentServicesP>();
            services.AddScoped<ServiceOrderServiceP>();

            return services;
        }
    }
}
