using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services_BE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_BE.Services
{
    public class ServiceOrderBackgroundService: BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ServiceOrderBackgroundService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public ServiceOrderBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<ServiceOrderBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ServiceOrderBackgroundService is running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var serviceOrderService = scope.ServiceProvider.GetRequiredService<IServiceOrderService>();
                        await serviceOrderService.UpdateExpiredOrdersAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in ServiceOrderBackgroundService: {ex.Message}");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
