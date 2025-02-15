﻿namespace WebAPI_BE.MiddleWare
{
    public class UserStatusMiddleware : IMiddleware
    {
        private readonly ILogger<UserStatusMiddleware> logger;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserStatusMiddleware(ILogger<UserStatusMiddleware> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);
        }
    }
}
