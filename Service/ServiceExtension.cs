using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public static class ServiceExtension
    {
        public static IServiceCollection UseContextRequestService(this IServiceCollection services)
        {
            return services.AddSingleton<ContextRequest>();
        }
    }
}
